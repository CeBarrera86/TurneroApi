using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Puesto;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class PuestoService : IPuestoService
{
  private readonly TurneroDbContext _context;
  private readonly IMapper _mapper;

  public PuestoService(TurneroDbContext context, IMapper mapper)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<Puesto?> GetPuestoAsync(int id)
  {
    return await _context.Puestos
        .Include(p => p.UsuarioNavigation)
        .Include(p => p.MostradorNavigation)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);
  }

  public async Task<(Puesto? puesto, string? errorMessage)> CreatePuestoAsync(PuestoCrearDto dto)
  {
    var usuarioError = await PuestoValidator.ValidateUsuarioExisteAsync(_context, dto.UsuarioId);
    if (usuarioError != null) return (null, usuarioError);

    var mostradorError = await PuestoValidator.ValidateMostradorExisteAsync(_context, dto.MostradorId);
    if (mostradorError != null) return (null, mostradorError);

    var dupError = await PuestoValidator.ValidateDuplicadoAsync(_context, dto.UsuarioId, dto.MostradorId);
    if (dupError != null) return (null, dupError);

    var puesto = new Puesto
    {
      UsuarioId = dto.UsuarioId,
      MostradorId = dto.MostradorId,
      Activo = dto.Activo,
      Login = dto.Activo ? DateTime.Now : null,
      Logout = null
    };

    _context.Puestos.Add(puesto);

    try
    {
      await _context.SaveChangesAsync();
      await _context.Entry(puesto).Reference(p => p.UsuarioNavigation).LoadAsync();
      await _context.Entry(puesto).Reference(p => p.MostradorNavigation).LoadAsync();
      return (puesto, null);
    }
    catch (DbUpdateException)
    {
      return (null, "Error al crear el puesto. Verificá los datos.");
    }
  }

  public async Task<(Puesto? puesto, string? errorMessage)> UpdatePuestoAsync(int id, PuestoActualizarDto dto)
  {
    var puesto = await _context.Puestos.FindAsync(id);
    if (puesto == null) return (null, "Puesto no encontrado.");

    // Solo se actualiza el estado de activo y fechas
    if (dto.Activo)
    {
      puesto.Login = DateTime.Now;
      puesto.Logout = null;
      puesto.Activo = true;
    }
    else
    {
      puesto.Logout = DateTime.Now;
      puesto.Activo = false;
    }

    try
    {
      await _context.SaveChangesAsync();
      await _context.Entry(puesto).Reference(p => p.UsuarioNavigation).LoadAsync();
      await _context.Entry(puesto).Reference(p => p.MostradorNavigation).LoadAsync();
      return (puesto, null);
    }
    catch (DbUpdateConcurrencyException)
    {
      if (!await _context.Puestos.AnyAsync(p => p.Id == id))
        return (null, "Puesto no encontrado (error de concurrencia).");
      throw;
    }
    catch (DbUpdateException)
    {
      return (null, "Error al actualizar el puesto. Verificá los datos.");
    }
  }
}