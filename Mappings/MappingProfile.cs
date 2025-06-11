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
                .ForMember(dest => dest.SectorNombre, opt => opt.MapFrom(src => src.SectorNavigation != null ? src.SectorNavigation.Nombre : null))
                .ReverseMap();

            CreateMap<MostradorCrearDto, Mostrador>().ReverseMap();

            CreateMap<MostradorActualizarDto, Mostrador>().ReverseMap();

            // Puesto
            CreateMap<Puesto, PuestoDto>()
            .ForMember(dest => dest.MostradorNavigation, opt => opt.MapFrom(src => src.MostradorNavigation))
            .ForMember(dest => dest.UsuarioNavigation, opt => opt.MapFrom(src => src.UsuarioNavigation));
        
            CreateMap<PuestoCrearDto, Puesto>()
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Turno, opt => opt.Ignore())
                .ForMember(dest => dest.MostradorNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Login, opt => opt.Ignore())
                .ForMember(dest => dest.Logout, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore());

            CreateMap<PuestoActualizarDto, Puesto>()
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Turno, opt => opt.Ignore())
                .ForMember(dest => dest.MostradorNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Login, opt => opt.Ignore())
                .ForMember(dest => dest.Logout, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore());

            // Rol
            CreateMap<Rol, RolDto>().ReverseMap();

            CreateMap<RolCrearDto, Rol>().ReverseMap();

            CreateMap<RolActualizarDto, Rol>().ReverseMap();

            // Sector
            CreateMap<Sector, SectorDto>()
                .ForMember(dest => dest.Padre, opt => opt.MapFrom(src => src.Padre != null ? src.Padre.Nombre : null))
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

            CreateMap<UsuarioCrearDto, Usuario>()
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Puesto, opt => opt.Ignore())
                .ForMember(dest => dest.RolNavigation, opt => opt.Ignore());

            CreateMap<UsuarioActualizarDto, Usuario>()
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Puesto, opt => opt.Ignore())
                .ForMember(dest => dest.RolNavigation, opt => opt.Ignore());
        }
    }
}
