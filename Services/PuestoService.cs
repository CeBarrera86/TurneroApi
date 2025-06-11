using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
    public class PuestoService : IPuestoService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public PuestoService(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Puesto?> GetPuestoAsync(uint id)
        {
            return await _context.Puestos.Include(p => p.MostradorNavigation).Include(p => p.UsuarioNavigation).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<(Puesto? puesto, string? errorMessage)> CreatePuestoAsync(PuestoCrearDto puestoCrearDto)
        {
            // Validar MostradorId
            var mostradorExiste = await _context.Mostradores.AnyAsync(m => m.Id == puestoCrearDto.MostradorId);
            if (!mostradorExiste)
            {
                return (null, $"El MostradorId '{puestoCrearDto.MostradorId}' proporcionado no es válido.");
            }

            // Validar UsuarioId
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == puestoCrearDto.UsuarioId);
            if (!usuarioExiste)
            {
                return (null, $"El UsuarioId '{puestoCrearDto.UsuarioId}' proporcionado no es válido.");
            }

            var mostradorActivoExistente = await _context.Puestos
                                                    .FirstOrDefaultAsync(p => p.MostradorId == puestoCrearDto.MostradorId && p.Activo == true);
            if (mostradorActivoExistente != null)
            {
                return (null, $"El mostrador con ID '{puestoCrearDto.MostradorId}' ya está asignado a un puesto activo (Puesto ID: {mostradorActivoExistente.Id}).");
            }
            
            var usuarioActivoExistente = await _context.Puestos
                                                .FirstOrDefaultAsync(p => p.UsuarioId == puestoCrearDto.UsuarioId && p.Activo == true);
            if (usuarioActivoExistente != null)
            {
                return (null, $"El usuario con ID '{puestoCrearDto.UsuarioId}' ya está logueado en un puesto activo (Puesto ID: {usuarioActivoExistente.Id}).");
            }

            var puesto = _mapper.Map<Puesto>(puestoCrearDto);
            
            puesto.Activo = true;
            puesto.Login = DateTime.Now;
            puesto.Logout = null;

            _context.Puestos.Add(puesto);

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(puesto).Reference(p => p.MostradorNavigation).LoadAsync();
                await _context.Entry(puesto).Reference(p => p.UsuarioNavigation).LoadAsync();
                return (puesto, null);
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al crear el puesto: {ex.Message}");
            }
        }

        public async Task<(Puesto? puesto, string? errorMessage)> UpdatePuestoAsync(uint id, PuestoActualizarDto puestoActualizarDto)
        {
            var puesto = await _context.Puestos.FirstOrDefaultAsync(p => p.Id == id);
            if (puesto == null)
            {
                return (null, "Puesto no encontrado.");
            }

            if (puestoActualizarDto.MostradorId != 0 && puestoActualizarDto.MostradorId != puesto.MostradorId)
            {
                var mostradorExiste = await _context.Mostradores.AnyAsync(m => m.Id == puestoActualizarDto.MostradorId);
                if (!mostradorExiste)
                {
                    return (null, $"El MostradorId '{puestoActualizarDto.MostradorId}' proporcionado no es válido.");
                }
                var mostradorActivoExistente = await _context.Puestos
                                                        .FirstOrDefaultAsync(p => p.MostradorId == puestoActualizarDto.MostradorId && p.Activo == true && p.Id != id);
                if (mostradorActivoExistente != null)
                {
                    return (null, $"El mostrador con ID '{puestoActualizarDto.MostradorId}' ya está asignado a otro puesto activo (Puesto ID: {mostradorActivoExistente.Id}).");
                }
                puesto.MostradorId = puestoActualizarDto.MostradorId;
            }

            if (puestoActualizarDto.UsuarioId != 0 && puestoActualizarDto.UsuarioId != puesto.UsuarioId)
            {
                var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == puestoActualizarDto.UsuarioId);
                if (!usuarioExiste)
                {
                    return (null, $"El UsuarioId '{puestoActualizarDto.UsuarioId}' proporcionado no es válido.");
                }
                var usuarioActivoExistente = await _context.Puestos
                                                    .FirstOrDefaultAsync(p => p.UsuarioId == puestoActualizarDto.UsuarioId && p.Activo == true && p.Id != id);
                if (usuarioActivoExistente != null)
                {
                    return (null, $"El usuario con ID '{puestoActualizarDto.UsuarioId}' ya está logueado en otro puesto activo (Puesto ID: {usuarioActivoExistente.Id}).");
                }
                puesto.UsuarioId = puestoActualizarDto.UsuarioId;
            }

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(puesto).Reference(p => p.MostradorNavigation).LoadAsync();
                await _context.Entry(puesto).Reference(p => p.UsuarioNavigation).LoadAsync();
                return (puesto, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Puestos.AnyAsync(e => e.Id == id))
                {
                    return (null, "Puesto no encontrado (error de concurrencia).");
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al actualizar el puesto: {ex.Message}");
            }
        }

        public async Task<(Puesto? puesto, string? errorMessage)> RegistrarLoginAsync(uint puestoId, uint usuarioId)
        {
            var puesto = await _context.Puestos.FirstOrDefaultAsync(p => p.Id == puestoId);
            if (puesto == null)
            {
                return (null, "Puesto no encontrado.");
            }

            // Validar que el usuario exista
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == usuarioId);
            if (!usuarioExiste)
            {
                return (null, $"El UsuarioId '{usuarioId}' proporcionado no es válido.");
            }

            var mostradorActivoExistente = await _context.Puestos
                                                    .FirstOrDefaultAsync(p => p.MostradorId == puesto.MostradorId && p.Activo == true && p.Id != puestoId);
            if (mostradorActivoExistente != null)
            {
                return (null, $"El mostrador asignado a este puesto (ID: {puesto.MostradorId}) ya está activo en otro puesto (Puesto ID: {mostradorActivoExistente.Id}).");
            }
            
            var usuarioActivoExistente = await _context.Puestos
                                                .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId && p.Activo == true && p.Id != puestoId);
            if (usuarioActivoExistente != null)
            {
                return (null, $"El usuario con ID '{usuarioId}' ya está logueado en otro puesto activo (Puesto ID: {usuarioActivoExistente.Id}).");
            }

            puesto.UsuarioId = usuarioId;
            puesto.Login = DateTime.Now;
            puesto.Logout = null;
            puesto.Activo = true;

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(puesto).Reference(p => p.MostradorNavigation).LoadAsync();
                await _context.Entry(puesto).Reference(p => p.UsuarioNavigation).LoadAsync();
                return (puesto, null);
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al registrar el login: {ex.Message}");
            }
        }

        public async Task<(Puesto? puesto, string? errorMessage)> RegistrarLogoutAsync(uint puestoId)
        {
            var puesto = await _context.Puestos.FirstOrDefaultAsync(p => p.Id == puestoId);
            if (puesto == null)
            {
                return (null, "Puesto no encontrado.");
            }

            if (puesto.Activo == false)
            {
                return (null, "El puesto ya se encuentra inactivo.");
            }

            puesto.Logout = DateTime.Now;
            puesto.Activo = false;

            try
            {
                await _context.SaveChangesAsync();
                await _context.Entry(puesto).Reference(p => p.MostradorNavigation).LoadAsync();
                await _context.Entry(puesto).Reference(p => p.UsuarioNavigation).LoadAsync();
                return (puesto, null);
            }
            catch (DbUpdateException ex)
            {
                return (null, $"Error al registrar el logout: {ex.Message}");
            }
        }
    }
}