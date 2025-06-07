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
                .ForMember(dest => dest.SectorNombre, opt => opt.MapFrom(src => src.SectorNavigation.Nombre))
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.EstadoNavigation.Descripcion));
            CreateMap<TicketDto, Ticket>();

            // Estado
            CreateMap<Estado, EstadoDto>().ReverseMap();

            // Historial
            CreateMap<Historial, HistorialDto>().ReverseMap();

            // Mostrador
            CreateMap<Mostrador, MostradorDto>().ReverseMap();

            // Puesto
            CreateMap<Puesto, PuestoDto>().ReverseMap();

            // Rol
            CreateMap<Rol, RolDto>().ReverseMap();

            // Sector
            CreateMap<Sector, SectorDto>().ReverseMap();

            // Tarea
            CreateMap<Tarea, TareaDto>().ReverseMap();

            // Turno
            CreateMap<Turno, TurnoDto>().ReverseMap();

            // User
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RolTipo, opt => opt.MapFrom(src => src.RoleNavigation.Tipo));
            CreateMap<UserDto, User>();

            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.EmailVerifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RememberToken, opt => opt.Ignore())
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Puesto, opt => opt.Ignore())
                .ForMember(dest => dest.RoleNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.EmailVerifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RememberToken, opt => opt.Ignore())
                .ForMember(dest => dest.Historial, opt => opt.Ignore())
                .ForMember(dest => dest.Puesto, opt => opt.Ignore())
                .ForMember(dest => dest.RoleNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
