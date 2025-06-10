using AutoMapper;
using TurneroApi.Models;
using TurneroApi.DTOs;

namespace TurneroApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Cliente
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            // Ticket
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.ClienteNavigation.Titular))
                .ForMember(dest => dest.SectorOrigenNombre, opt => opt.MapFrom(src => src.SectorIdOrigenNavigation.Nombre))
                .ForMember(dest => dest.SectorActualNombre, opt => opt.MapFrom(src => src.SectorIdActualNavigation.Nombre))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.EstadoNavigation.Descripcion));
            CreateMap<TicketDto, Ticket>();
            // Estado
            CreateMap<Estado, EstadoDto>().ReverseMap();
            // Historial
            CreateMap<Historial, HistorialDto>().ReverseMap();
            // Mostrador
            CreateMap<Mostrador, MostradorDto>()
                .ForMember(dest => dest.SectorNombre, opt => opt.MapFrom(src => src.SectorNavigation.Nombre))
                .ReverseMap();
            // Puesto
            CreateMap<Puesto, PuestoDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.UsuarioNavigation.Nombre)) // Ajustar si es otro campo
                .ForMember(dest => dest.MostradorNumero, opt => opt.MapFrom(src => src.MostradorNavigation.Numero))
                .ReverseMap();
            // Rol
            CreateMap<Rol, RolDto>().ReverseMap();
            // Sector
            CreateMap<Sector, SectorDto>()
                .ForMember(dest => dest.PadreNombre, opt => opt.MapFrom(src => src.Padre!.Nombre))
                .ReverseMap();
            CreateMap<SectorDto, Sector>();
            // Turno
            CreateMap<Turno, TurnoDto>().ReverseMap();
            // Usuario
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.RolTipo, opt => opt.MapFrom(src => src.RolNavigation.Tipo));
            CreateMap<UsuarioDto, Usuario>();
        }
    }
}
