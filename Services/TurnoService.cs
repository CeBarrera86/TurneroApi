using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Turno;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
  public class TurnoService : ITurnoService
  {
    private readonly TurneroDbContext _context;
    private readonly IMapper _mapper;

    public TurnoService(TurneroDbContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public async Task<List<TurnoDto>> GetTurnosAsync()
    {
      return await _context.Turnos
        .AsNoTracking()
        .OrderByDescending(t => t.FechaInicio)
        .ProjectTo<TurnoDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<TurnoDto?> GetTurnoAsync(ulong id)
    {
      return await _context.Turnos
        .AsNoTracking()
        .Where(t => t.Id == id)
        .ProjectTo<TurnoDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
    }

    public async Task<(Turno? turno, string? errorMessage)> CreateTurnoAsync(TurnoCrearDto turnoCrearDto, int puestoId)
    {
      // Validaciones de existencia
      var puestoExiste = await _context.Puestos.AnyAsync(p => p.Id == puestoId);
      if (!puestoExiste)
        return (null, $"El PuestoId '{puestoId}' no existe.");

      var ticketExiste = await _context.Tickets.AnyAsync(t => t.Id == turnoCrearDto.TicketId);
      if (!ticketExiste)
        return (null, $"El TicketId '{turnoCrearDto.TicketId}' no existe.");

      // Estado ATENDIDO (ID = 1)
      var estadoAtendido = await _context.Estados.FirstOrDefaultAsync(e => e.Id == 1 && e.Descripcion == "ATENDIDO");
      if (estadoAtendido == null)
        return (null, "El estado 'ATENDIDO' (ID 1) no está configurado en la base de datos.");

      // Un puesto no puede tener más de un turno atendido activo
      var turnoActivoExistente = await _context.Turnos
          .AnyAsync(t => t.PuestoId == puestoId && t.EstadoId == estadoAtendido.Id);
      if (turnoActivoExistente)
        return (null, $"El puesto '{puestoId}' ya tiene un turno 'ATENDIDO' activo.");

      // Un ticket no puede estar atendido en múltiples turnos simultáneamente
      var ticketAtendidoExistente = await _context.Turnos
          .AnyAsync(t => t.TicketId == turnoCrearDto.TicketId && t.EstadoId == estadoAtendido.Id);
      if (ticketAtendidoExistente)
        return (null, $"El Ticket '{turnoCrearDto.TicketId}' ya está siendo atendido en otro turno.");

      var turno = _mapper.Map<Turno>(turnoCrearDto);
      turno.PuestoId = puestoId;
      turno.FechaInicio = DateTime.Now;
      turno.EstadoId = estadoAtendido.Id;
      turno.FechaFin = null;

      _context.Turnos.Add(turno);

      try
      {
        await _context.SaveChangesAsync();

        await _context.Entry(turno).Reference(t => t.PuestoNavigation).LoadAsync();
        await _context.Entry(turno).Reference(t => t.TicketNavigation).LoadAsync();
        await _context.Entry(turno).Reference(t => t.EstadoNavigation).LoadAsync();

        return (turno, null);
      }
      catch (DbUpdateException ex)
      {
        return (null, $"Error al crear el turno: {ex.Message}");
      }
    }

    public async Task<(Turno? turno, string? errorMessage)> UpdateTurnoAsync(ulong id, TurnoActualizarDto turnoActualizarDto)
    {
      var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == id);
      if (turno == null)
      {
        return (null, "Turno no encontrado.");
      }

      bool changed = false;

      // --- Lógica para actualizar EstadoId ---
      if (turnoActualizarDto.EstadoId.HasValue && turnoActualizarDto.EstadoId.Value != turno.EstadoId)
      {
        var newEstadoId = turnoActualizarDto.EstadoId.Value;

        // Validar que el EstadoId proporcionado exista
        var estadoExiste = await _context.Estados.AnyAsync(e => e.Id == newEstadoId);
        if (!estadoExiste)
        {
          return (null, $"El EstadoId '{newEstadoId}' proporcionado no es válido.");
        }

        // Definir los estados que finalizan un turno: CULMINADO (2), DERIVADO (3), ELIMINADO (5)
        bool isCurrentStateFinal = (turno.EstadoId == 2 || turno.EstadoId == 3 || turno.EstadoId == 5);
        bool isNewStateFinal = (newEstadoId == 2 || newEstadoId == 3 || newEstadoId == 5);

        // Regla: No se puede volver a ATENDIDO (1) desde un estado finalizado.
        if (isCurrentStateFinal && newEstadoId == 1)
        {
          return (null, "No se puede cambiar un turno ya finalizado a 'ATENDIDO'.");
        }

        // Si el nuevo estado es ATENDIDO (1), verifica que el puesto no tenga otro turno ATENDIDO.
        if (newEstadoId == 1)
        {
          var estadoAtendido = await _context.Estados.FirstOrDefaultAsync(e => e.Id == 1 && e.Descripcion == "ATENDIDO");
          if (estadoAtendido == null) return (null, "El estado 'ATENDIDO' no está configurado en la base de datos.");

          var turnoActivoExistente = await _context.Turnos
                                                  .AnyAsync(t => t.PuestoId == turno.PuestoId && t.EstadoId == estadoAtendido.Id && t.Id != turno.Id);
          if (turnoActivoExistente)
          {
            return (null, $"El puesto '{turno.PuestoId}' ya tiene otro turno 'ATENDIDO' activo. Finalice el turno actual antes de reactivar uno nuevo.");
          }
        }

        turno.EstadoId = newEstadoId;
        changed = true;

        // Si el nuevo estado finaliza el turno, establece FechaFin si no está ya establecida.
        if (isNewStateFinal)
        {
          if (!turno.FechaFin.HasValue) // Solo si FechaFin no ha sido establecida
          {
            turno.FechaFin = DateTime.Now;
            changed = true; // Marca como cambiado si se actualizó FechaFin
          }
        }
        // Si el nuevo estado NO es final y FechaFin tiene un valor, nulifícala (ej. si se pasa de Culminado a Atendido, lo cual está bloqueado, pero es una buena regla general)
        else
        {
          if (turno.FechaFin.HasValue)
          {
            turno.FechaFin = null;
            changed = true; // Marca como cambiado si se nulificó FechaFin
          }
        }
      }

      // --- Lógica para actualizar FechaFin ---
      // Esto solo se aplica si el cliente envía explícitamente una FechaFin.
      // La lógica de EstadoId ya puede establecer FechaFin automáticamente.
      if (turnoActualizarDto.FechaFin.HasValue && turnoActualizarDto.FechaFin.Value != turno.FechaFin)
      {
        // Regla de negocio: FechaFin no puede ser anterior a FechaInicio.
        if (turnoActualizarDto.FechaFin.Value < turno.FechaInicio)
        {
          return (null, "La FechaFin no puede ser anterior a la FechaInicio del turno.");
        }

        // Si el cliente envía una FechaFin explícitamente, úsala.
        // Podrías añadir una regla aquí para evitar sobrescribir FechaFin si el estado NO es final
        // O permitir que la ponga el usuario, siempre que sea posterior a FechaInicio.
        turno.FechaFin = turnoActualizarDto.FechaFin.Value;
        changed = true;
      }

      // Actualizar 'Actualizado' (si tu modelo Turno tiene esta propiedad)
      // if (changed)
      // {
      //    turno.Actualizado = DateTime.Now;
      // }

      if (!changed)
      {
        return (turno, null); // No hubo cambios, devuelve el turno sin modificar.
      }

      try
      {
        await _context.SaveChangesAsync();
        // Cargar navegaciones para el DTO de respuesta
        await _context.Entry(turno).Reference(t => t.PuestoNavigation).LoadAsync();
        await _context.Entry(turno).Reference(t => t.TicketNavigation).LoadAsync();
        await _context.Entry(turno).Reference(t => t.EstadoNavigation).LoadAsync();
        return (turno, null);
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!await _context.Turnos.AnyAsync(e => e.Id == id))
        {
          return (null, "Turno no encontrado (error de concurrencia).");
        }
        throw;
      }
      catch (DbUpdateException ex)
      {
        return (null, $"Error al actualizar el turno: {ex.Message}");
      }
    }

    // Nuevo método para obtener un turno activo (ATENDIDO) por PuestoId
    public async Task<TurnoDto?> GetTurnoActivoPorPuestoIdAsync(int puestoId)
    {
      // Consideramos "activo" como estado ATENDIDO (ID = 1). Ajusta si necesitás otra definición.
      var estadoAtendidoId = 1;

      return await _context.Turnos
          .AsNoTracking()
          .Where(t => t.PuestoId == puestoId && t.EstadoId == estadoAtendidoId && t.FechaFin == null)
          .ProjectTo<TurnoDto>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();
    }
  }
}