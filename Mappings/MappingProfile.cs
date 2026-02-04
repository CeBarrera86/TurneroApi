using AutoMapper;
using TurneroApi.Models;
using TurneroApi.DTOs;
using TurneroApi.DTOs.Cliente;
using TurneroApi.DTOs.Contenido;
using TurneroApi.DTOs.Estado;
using TurneroApi.DTOs.Historial;
using TurneroApi.DTOs.Mostrador;
using TurneroApi.DTOs.MostradorSector;
using TurneroApi.DTOs.Permiso;
using TurneroApi.DTOs.Puesto;
using TurneroApi.DTOs.Rol;
using TurneroApi.DTOs.RolPermiso;
using TurneroApi.DTOs.Sector;
using TurneroApi.DTOs.Ticket;
using TurneroApi.DTOs.Turno;
using TurneroApi.DTOs.Usuario;

namespace TurneroApi.Mappings
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      // Cliente
      CreateMap<Cliente, ClienteDto>().ReverseMap();
      CreateMap<ClienteCrearDto, Cliente>().ReverseMap();

      // Contenido
      CreateMap<Contenido, ContenidoDto>().AfterMap<ContenidoMappingAction>();
      CreateMap<ContenidoCrearDto, Contenido>().ReverseMap();
      CreateMap<ContenidoActualizarDto, Contenido>().ReverseMap();

      // Estado
      CreateMap<Estado, EstadoDto>().ReverseMap();
      CreateMap<EstadoCrearDto, Estado>().ReverseMap();
      CreateMap<EstadoActualizarDto, Estado>().ReverseMap();

      // Historial
      CreateMap<Historial, HistorialDto>()
        .ForMember(dest => dest.TicketNavigation, opt => opt.MapFrom(src => src.TicketNavigation))
        .ForMember(dest => dest.EstadoNavigation, opt => opt.MapFrom(src => src.EstadoNavigation))
        .ForMember(dest => dest.PuestoNavigation, opt => opt.MapFrom(src => src.PuestoNavigation))
        .ForMember(dest => dest.TurnoNavigation, opt => opt.MapFrom(src => src.TurnoNavigation))
        .ForMember(dest => dest.UsuarioNavigation, opt => opt.MapFrom(src => src.UsuarioNavigation));
      CreateMap<HistorialCrearDto, Historial>().ReverseMap();

      // Mostrador
      CreateMap<Mostrador, MostradorDto>();
      CreateMap<MostradorCrearDto, Mostrador>().ReverseMap();
      CreateMap<MostradorActualizarDto, Mostrador>().ReverseMap();

      // MostradorSector
      CreateMap<MostradorSector, MostradorSectorDto>().ReverseMap();
      CreateMap<MostradorSectorCrearDto, MostradorSector>().ReverseMap();

      // Permiso
      CreateMap<Permiso, PermisoDto>().ReverseMap();
      CreateMap<PermisoCrearDto, Permiso>().ReverseMap();
      CreateMap<PermisoActualizarDto, Permiso>().ReverseMap();

      // Puesto
      CreateMap<Puesto, PuestoDto>()
        .ForMember(dest => dest.UsuarioNombre,
          opt => opt.MapFrom(src => src.UsuarioNavigation != null
            ? src.UsuarioNavigation.Nombre + " " + src.UsuarioNavigation.Apellido
            : string.Empty))
        .ForMember(dest => dest.MostradorIp,
          opt => opt.MapFrom(src => src.MostradorNavigation != null
            ? src.MostradorNavigation.Ip
            : string.Empty));
      CreateMap<PuestoCrearDto, Puesto>().ReverseMap();
      CreateMap<PuestoActualizarDto, Puesto>().ReverseMap();

      // Rol
      CreateMap<Rol, RolDto>().ReverseMap();
      CreateMap<RolCrearDto, Rol>().ReverseMap();
      CreateMap<RolActualizarDto, Rol>().ReverseMap();

      // RolPermiso
      CreateMap<RolPermiso, RolPermisoDto>().ReverseMap();
      CreateMap<RolPermisoCrearDto, RolPermiso>().ReverseMap();

      // Sector
      CreateMap<Sector, SectorDto>()
        .ForMember(dest => dest.PadreId, opt => opt.MapFrom(src => src.PadreId));
      CreateMap<SectorCrearDto, Sector>().ReverseMap();
      CreateMap<SectorActualizarDto, Sector>().ReverseMap();

      // Ticket
      CreateMap<Ticket, TicketDto>()
        .ForMember(dest => dest.ClienteNavigation, opt => opt.MapFrom(src => src.ClienteNavigation))
        .ForMember(dest => dest.EstadoNavigation, opt => opt.MapFrom(src => src.EstadoNavigation))
        .ForMember(dest => dest.SectorIdOrigenNavigation, opt => opt.MapFrom(src => src.SectorIdOrigenNavigation))
        .ForMember(dest => dest.SectorIdActualNavigation, opt => opt.MapFrom(src => src.SectorIdActualNavigation));
      CreateMap<TicketCrearDto, Ticket>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Numero, opt => opt.Ignore())
        .ForMember(dest => dest.Fecha, opt => opt.Ignore())
        .ForMember(dest => dest.Actualizado, opt => opt.Ignore())
        .ForMember(dest => dest.SectorIdActual, opt => opt.Ignore())
        .ForMember(dest => dest.EstadoId, opt => opt.Ignore())
        .ForMember(dest => dest.Historiales, opt => opt.Ignore())
        .ForMember(dest => dest.Turnos, opt => opt.Ignore());
      CreateMap<TicketActualizarDto, Ticket>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.Letra, opt => opt.Ignore())
        .ForMember(dest => dest.Numero, opt => opt.Ignore())
        .ForMember(dest => dest.ClienteId, opt => opt.Ignore())
        .ForMember(dest => dest.Fecha, opt => opt.Ignore())
        .ForMember(dest => dest.SectorIdOrigen, opt => opt.Ignore())
        .ForMember(dest => dest.Historiales, opt => opt.Ignore())
        .ForMember(dest => dest.Turnos, opt => opt.Ignore())
        .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

      // Turno
      CreateMap<Turno, TurnoDto>()
        .ForMember(dest => dest.PuestoNavigation, opt => opt.MapFrom(src => src.PuestoNavigation))
        .ForMember(dest => dest.TicketNavigation, opt => opt.MapFrom(src => src.TicketNavigation))
        .ForMember(dest => dest.EstadoNavigation, opt => opt.MapFrom(src => src.EstadoNavigation));
      CreateMap<TurnoCrearDto, Turno>().ReverseMap();
      CreateMap<TurnoActualizarDto, Turno>().ReverseMap();

      // Usuario
      CreateMap<Usuario, UsuarioDto>()
        .ForMember(dest => dest.RolNombre, opt => opt.MapFrom(src => src.RolNavigation.Nombre));
      CreateMap<UsuarioCrearDto, Usuario>().ReverseMap();
      CreateMap<UsuarioActualizarDto, Usuario>().ReverseMap();
    }
  }
}