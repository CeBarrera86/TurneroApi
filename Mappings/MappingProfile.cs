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

            // Estado
            CreateMap<Estado, EstadoDto>().ReverseMap();

            CreateMap<EstadoCrearDto, Estado>().ReverseMap();

            CreateMap<EstadoActualizarDto, Estado>()
                .ForMember(dest => dest.Letra, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Letra)))
                .ForMember(dest => dest.Descripcion, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Descripcion)));
            CreateMap<Estado, EstadoActualizarDto>();

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

            CreateMap<RolCrearDto, Rol>().ReverseMap();

            CreateMap<RolActualizarDto, Rol>().ReverseMap();

            // Sector
            CreateMap<Sector, SectorDto>()
                .ForMember(dest => dest.PadreNombre, opt => opt.MapFrom(src => src.Padre != null ? src.Padre.Nombre : null))
                .ReverseMap();

            CreateMap<SectorCrearDto, Sector>().ReverseMap();

            CreateMap<SectorActualizarDto, Sector>().ReverseMap();

            // Ticket
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.ClienteNavigation.Titular))
                .ForMember(dest => dest.SectorOrigenNombre, opt => opt.MapFrom(src => src.SectorIdOrigenNavigation.Nombre))
                .ForMember(dest => dest.SectorActualNombre, opt => opt.MapFrom(src => src.SectorIdActualNavigation.Nombre))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.EstadoNavigation.Descripcion));
            CreateMap<TicketDto, Ticket>();

            // Turno
            CreateMap<Turno, TurnoDto>().ReverseMap();

            // Usuario
            CreateMap<Usuario, UsuarioDto>()
                .ForMember(dest => dest.RolTipo, opt => opt.MapFrom(src => src.RolNavigation.Tipo));
            CreateMap<UsuarioDto, Usuario>();

            CreateMap<UsuarioCrearDto, Usuario>()
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Puesto, opt => opt.Ignore())
                .ForMember(dest => dest.RolNavigation, opt => opt.Ignore());
            CreateMap<Usuario, UsuarioCrearDto>();

            CreateMap<UsuarioActualizarDto, Usuario>()
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Puesto, opt => opt.Ignore())
                .ForMember(dest => dest.RolNavigation, opt => opt.Ignore());
            CreateMap<Usuario, UsuarioActualizarDto>();
        }
    }
}
