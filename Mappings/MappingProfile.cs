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
            CreateMap<ClienteCrearDto, Cliente>();
            CreateMap<Cliente, ClienteDto>();

            // Si usas el ClienteRemotoDto para la búsqueda remota, mapea también:
            // CreateMap<IClienteRemotoService.ClienteRemotoDto, ClienteCrearDto>();
            // CreateMap<IClienteRemotoService.ClienteRemotoDto, Cliente>();

            // Estado
            CreateMap<Estado, EstadoDto>().ReverseMap();

            CreateMap<EstadoCrearDto, Estado>().ReverseMap();

            CreateMap<EstadoActualizarDto, Estado>()
                .ForMember(dest => dest.Letra, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Letra)))
                .ForMember(dest => dest.Descripcion, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Descripcion)));
            CreateMap<Estado, EstadoActualizarDto>();

            // Historial
            CreateMap<HistorialCrearDto, Historial>();

            CreateMap<Historial, HistorialDto>()
                .ForMember(dest => dest.TicketNavigation, opt => opt.MapFrom(src => src.TicketNavigation))
                .ForMember(dest => dest.EstadoNavigation, opt => opt.MapFrom(src => src.EstadoNavigation))
                .ForMember(dest => dest.PuestoNavigation, opt => opt.MapFrom(src => src.PuestoNavigation))
                .ForMember(dest => dest.TurnoNavigation, opt => opt.MapFrom(src => src.TurnoNavigation))
                .ForMember(dest => dest.UsuarioNavigation, opt => opt.MapFrom(src => src.UsuarioNavigation));

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
            CreateMap<Sector, SectorDto>().ReverseMap();

            CreateMap<SectorCrearDto, Sector>().ReverseMap();

            CreateMap<SectorActualizarDto, Sector>().ReverseMap();

            // Ticket
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.ClienteNavigation, opt => opt.MapFrom(src => src.ClienteNavigation))
                .ForMember(dest => dest.EstadoNavigation, opt => opt.MapFrom(src => src.EstadoNavigation))
                .ForMember(dest => dest.SectorIdActualNavigation, opt => opt.MapFrom(src => src.SectorIdActualNavigation))
                .ForMember(dest => dest.SectorIdOrigenNavigation, opt => opt.MapFrom(src => src.SectorIdOrigenNavigation));

            CreateMap<TicketCrearDto, Ticket>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Numero, opt => opt.Ignore())
                .ForMember(dest => dest.Fecha, opt => opt.Ignore())
                .ForMember(dest => dest.Actualizado, opt => opt.Ignore())
                .ForMember(dest => dest.SectorIdActual, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoId, opt => opt.Ignore())
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Turno, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.SectorIdActualNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.SectorIdOrigenNavigation, opt => opt.Ignore());

            CreateMap<TicketActualizarDto, Ticket>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Letra, opt => opt.Ignore())
                .ForMember(dest => dest.Numero, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
                .ForMember(dest => dest.Fecha, opt => opt.Ignore())
                .ForMember(dest => dest.SectorIdOrigen, opt => opt.Ignore())
                .ForMember(dest => dest.Actualizado, opt => opt.Ignore())
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Turno, opt => opt.Ignore())
                .ForMember(dest => dest.ClienteNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.SectorIdActualNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.SectorIdOrigenNavigation, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // Turno
            CreateMap<TurnoCrearDto, Turno>();
            
            CreateMap<TurnoActualizarDto, Turno>()
                 .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<Turno, TurnoDto>()
                .ForMember(dest => dest.PuestoNavigation, opt => opt.MapFrom(src => src.PuestoNavigation))
                .ForMember(dest => dest.TicketNavigation, opt => opt.MapFrom(src => src.TicketNavigation))
                .ForMember(dest => dest.EstadoNavigation, opt => opt.MapFrom(src => src.EstadoNavigation));

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
