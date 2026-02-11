using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Ticket;
using TurneroApi.DTOs.Turno;
using TurneroApi.DTOs.Historial;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Enums;
using TurneroApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace TurneroApi.Services
{
  public class TicketService : ITicketService
  {
    private readonly TurneroDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITurnoService _turnoService;
    private readonly IHistorialService _historialService;
    private readonly IHubContext<TicketsHub> _hubContext;

    public TicketService(
        TurneroDbContext context,
        IMapper mapper,
        ITurnoService turnoService,
        IHistorialService historialService,
        IHubContext<TicketsHub> hubContext)
    {
      _context = context;
      _mapper = mapper;
      _turnoService = turnoService;
      _historialService = historialService;
      _hubContext = hubContext;
    }

    public async Task<List<TicketDto>> GetTicketsAsync()
    {
      return await _context.Tickets
          .AsNoTracking()
          .OrderByDescending(t => t.Fecha)
          .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
          .ToListAsync();
    }

    public async Task<TicketDto?> GetTicketAsync(ulong id)
    {
      return await _context.Tickets
          .AsNoTracking()
          .Where(t => t.Id == id)
          .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();
    }

    public async Task<List<TicketDto>> GetTicketsFiltrados(DateTime fecha, int sectorIdOrigen, int estadoId)
    {
      return await _context.Tickets
        .AsNoTracking()
        .Where(t =>
          t.Fecha.Date >= fecha.Date &&
          t.SectorIdOrigen == sectorIdOrigen &&
          t.EstadoId == estadoId
        )
        .OrderByDescending(t => t.Fecha)
        .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
    }

    public async Task<(Ticket? ticket, string? errorMessage)> CrearTicket(TicketCrearDto ticketCrearDto)
    {
      // 1. Obtener el sector de origen para derivar la letra
      var sectorOrigen = await _context.Sectores.FirstOrDefaultAsync(s => s.Id == ticketCrearDto.SectorIdOrigen);
      if (sectorOrigen == null)
      {
        return (null, $"El SectorIdOrigen '{ticketCrearDto.SectorIdOrigen}' proporcionado no es válido.");
      }
      if (string.IsNullOrWhiteSpace(sectorOrigen.Letra))
      {
        return (null, $"El sector '{sectorOrigen.Nombre}' (ID: {sectorOrigen.Id}) no tiene una letra asignada.");
      }

      string letra = sectorOrigen.Letra.ToUpper(); // Derivar la letra del sector
      var today = DateTime.Today;
      int numero = 0; // Empieza en 0 cada día

      // Validaciones existentes
      var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == ticketCrearDto.ClienteId);
      if (!clienteExiste)
      {
        return (null, $"El ClienteId '{ticketCrearDto.ClienteId}' proporcionado no es válido.");
      }

      var ultimoTicket = await _context.Tickets
          .Where(t => t.Letra == letra && t.Fecha.Date == today)
          .OrderByDescending(t => t.Numero)
          .FirstOrDefaultAsync();

      if (ultimoTicket != null)
      {
        numero = ultimoTicket.Numero + 1;
      }

      var ticketExistente = await _context.Tickets
          .AnyAsync(t => t.Letra == letra && t.Numero == numero && t.Fecha.Date == today);

      if (ticketExistente)
      {
        return (null, $"Ya existe un ticket con la combinación Letra '{letra}' y Número '{numero}' para la fecha actual. Intente nuevamente.");
      }

      var estadoDisponible = await _context.Estados.FirstOrDefaultAsync(e => e.Id == 4 && e.Descripcion == "DISPONIBLE");
      if (estadoDisponible == null)
      {
        return (null, "El estado 'DISPONIBLE' (ID 4) no está configurado en la base de datos.");
      }

      var ticket = _mapper.Map<Ticket>(ticketCrearDto); // Mapea las propiedades disponibles

      ticket.Letra = letra; // Asigna la letra derivada
      ticket.Numero = numero;
      ticket.ClienteId = ticketCrearDto.ClienteId;
      ticket.Fecha = DateTime.Now;
      ticket.SectorIdOrigen = ticketCrearDto.SectorIdOrigen;
      ticket.EstadoId = 4; // DISPONIBLE

      _context.Tickets.Add(ticket);

      try
      {
        await _context.SaveChangesAsync();

        // Cargar navegaciones para el DTO de respuesta
        await _context.Entry(ticket).Reference(t => t.ClienteNavigation).LoadAsync();
        await _context.Entry(ticket).Reference(t => t.EstadoNavigation).LoadAsync();
        if (ticket.SectorIdActual.HasValue)
        {
          await _context.Entry(ticket).Reference(t => t.SectorIdActualNavigation!).LoadAsync();
        }
        await _context.Entry(ticket).Reference(t => t.SectorIdOrigenNavigation).LoadAsync();

        var ticketDto = _mapper.Map<TicketDto>(ticket);
        await _hubContext.Clients.All.SendAsync("ticketCreated", ticketDto);

        return (ticket, null);
      }
      catch (DbUpdateException ex)
      {
        return (null, $"Error al crear el ticket: {ex.Message}");
      }
    }

    public async Task<TicketDto?> LlamarTicketAsync(ulong id, int? usuarioId, int? puestoId)
    {
      var dto = new TicketActualizarDto { EstadoId = (int)EstadoTicket.ATENDIDO };
      var (ticket, _) = await ActualizarTicket(id, dto, usuarioId, puestoId);

      // Aquí podrías emitir un evento si tenés sistema de notificaciones
      // await _eventoService.Emitir("llamandoTicket", ticket.Id);

      return ticket == null ? null : _mapper.Map<TicketDto>(ticket);
    }

    public async Task<(Ticket? ticket, string? errorMessage)> ActualizarTicket(ulong id, TicketActualizarDto dto, int? usuarioId, int? puestoId)
    {
      var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
      if (ticket == null)
        return (null, "Ticket no encontrado.");

      bool huboCambios = false;

      if (dto.EstadoId.HasValue && dto.EstadoId.Value != ticket.EstadoId)
      {
        ticket.EstadoId = dto.EstadoId.Value;
        ticket.Actualizado = DateTime.Now;
        huboCambios = true;

        if ((EstadoTicket)dto.EstadoId.Value == EstadoTicket.ATENDIDO)
        {
          if (!puestoId.HasValue)
          {
            return (null, "PuestoId no disponible para crear el turno.");
          }

          await _turnoService.CreateTurnoAsync(new TurnoCrearDto
          {
            TicketId = ticket.Id
          }, puestoId.Value);
        }

        await _historialService.AddHistorialEntryAsync(new HistorialCrearDto
        {
          TicketId = ticket.Id,
          EstadoId = dto.EstadoId.Value,
          UsuarioId = usuarioId, // ahora int? directo
          Fecha = DateTime.Now,
          Comentarios = $"Cambio de estado a {(EstadoTicket)dto.EstadoId.Value}"
        });
      }

      if (dto.SectorIdActual.HasValue && dto.SectorIdActual.Value != ticket.SectorIdActual)
      {
        ticket.SectorIdActual = dto.SectorIdActual.Value;
        ticket.Actualizado = DateTime.Now;
        huboCambios = true;
      }

      if (!huboCambios)
        return (ticket, null);

      await _context.SaveChangesAsync();
      return (ticket, null);
    }

    public async Task<TicketDto?> BuscarTicket(string letra, int numero) // ahora int
    {
      var today = DateTime.Today;
      return await _context.Tickets
          .AsNoTracking()
          .Where(t => t.Letra == letra && t.Numero == numero && t.Fecha.Date == today)
          .ProjectTo<TicketDto>(_mapper.ConfigurationProvider)
          .FirstOrDefaultAsync();
    }
  }
}
