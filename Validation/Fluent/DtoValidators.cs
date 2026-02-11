using FluentValidation;
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
using TurneroApi.Models.Session;

namespace TurneroApi.Validation.Fluent;

public sealed class DtoValidators { }

public class ClienteCrearDtoValidator : AbstractValidator<ClienteCrearDto>
{
  public ClienteCrearDtoValidator()
  {
    RuleFor(x => x.Dni)
      .NotEmpty().WithMessage("El DNI es obligatorio.")
      .MaximumLength(10).WithMessage("El DNI no puede exceder los 10 caracteres.")
      .Matches("^[0-9]+$").WithMessage("El DNI debe contener solo números.");

    RuleFor(x => x.Titular)
      .NotEmpty().WithMessage("El titular es obligatorio.")
      .MaximumLength(50).WithMessage("El titular no puede exceder los 50 caracteres.");
  }
}

public class TicketCrearDtoValidator : AbstractValidator<TicketCrearDto>
{
  public TicketCrearDtoValidator()
  {
    RuleFor(x => x.ClienteId).GreaterThan(0UL);
    RuleFor(x => x.SectorIdOrigen).GreaterThan(0);
  }
}

public class TicketActualizarDtoValidator : AbstractValidator<TicketActualizarDto>
{
  public TicketActualizarDtoValidator()
  {
    RuleFor(x => x.SectorIdActual).GreaterThan(0).When(x => x.SectorIdActual.HasValue);
    RuleFor(x => x.EstadoId).GreaterThan(0).When(x => x.EstadoId.HasValue);
  }
}

public class TurnoCrearDtoValidator : AbstractValidator<TurnoCrearDto>
{
  public TurnoCrearDtoValidator()
  {
    RuleFor(x => x.TicketId).GreaterThan(0UL);
  }
}

public class TurnoActualizarDtoValidator : AbstractValidator<TurnoActualizarDto>
{
  public TurnoActualizarDtoValidator()
  {
    RuleFor(x => x.EstadoId).GreaterThan(0).When(x => x.EstadoId.HasValue);
    RuleFor(x => x.FechaFin).NotEmpty().When(x => x.FechaFin.HasValue);
  }
}

public class UsuarioCrearDtoValidator : AbstractValidator<UsuarioCrearDto>
{
  public UsuarioCrearDtoValidator()
  {
    RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Apellido).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Username).NotEmpty().MaximumLength(30);
    RuleFor(x => x.RolId).GreaterThan(0);
  }
}

public class UsuarioActualizarDtoValidator : AbstractValidator<UsuarioActualizarDto>
{
  public UsuarioActualizarDtoValidator()
  {
    RuleFor(x => x.Nombre).MaximumLength(50).When(x => x.Nombre != null);
    RuleFor(x => x.Apellido).MaximumLength(50).When(x => x.Apellido != null);
    RuleFor(x => x.Username).MaximumLength(30).When(x => x.Username != null);
    RuleFor(x => x.RolId).GreaterThan(0).When(x => x.RolId.HasValue);
  }
}

public class EstadoCrearDtoValidator : AbstractValidator<EstadoCrearDto>
{
  public EstadoCrearDtoValidator()
  {
    RuleFor(x => x.Letra)
      .NotEmpty()
      .MaximumLength(2)
      .Matches("^[A-Z]{1,2}$").WithMessage("La letra debe contener solo letras mayúsculas.");
    RuleFor(x => x.Descripcion).NotEmpty().MaximumLength(20);
  }
}

public class EstadoActualizarDtoValidator : AbstractValidator<EstadoActualizarDto>
{
  public EstadoActualizarDtoValidator()
  {
    RuleFor(x => x.Letra)
      .MaximumLength(2)
      .Matches("^[A-Z]{1,2}$").When(x => x.Letra != null);
    RuleFor(x => x.Descripcion).MaximumLength(20).When(x => x.Descripcion != null);
  }
}

public class RolCrearDtoValidator : AbstractValidator<RolCrearDto>
{
  public RolCrearDtoValidator()
  {
    RuleFor(x => x.Nombre).NotEmpty().MaximumLength(20);
  }
}

public class RolActualizarDtoValidator : AbstractValidator<RolActualizarDto>
{
  public RolActualizarDtoValidator()
  {
    RuleFor(x => x.Nombre).MaximumLength(20).When(x => x.Nombre != null);
  }
}

public class PermisoCrearDtoValidator : AbstractValidator<PermisoCrearDto>
{
  public PermisoCrearDtoValidator()
  {
    RuleFor(x => x.Nombre).NotEmpty().MaximumLength(50);
    RuleFor(x => x.Descripcion).MaximumLength(100).When(x => x.Descripcion != null);
  }
}

public class PermisoActualizarDtoValidator : AbstractValidator<PermisoActualizarDto>
{
  public PermisoActualizarDtoValidator()
  {
    RuleFor(x => x.Nombre).MaximumLength(50).When(x => x.Nombre != null);
    RuleFor(x => x.Descripcion).MaximumLength(100).When(x => x.Descripcion != null);
  }
}

public class MostradorCrearDtoValidator : AbstractValidator<MostradorCrearDto>
{
  public MostradorCrearDtoValidator()
  {
    RuleFor(x => x.Numero).GreaterThan(0);
    RuleFor(x => x.Ip).NotEmpty().Matches(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
      .WithMessage("El formato de la dirección IP no es válido. Debe ser IPv4 (ej. 192.168.1.1).");
    RuleFor(x => x.Tipo).MaximumLength(10).When(x => x.Tipo != null);
  }
}

public class MostradorActualizarDtoValidator : AbstractValidator<MostradorActualizarDto>
{
  public MostradorActualizarDtoValidator()
  {
    RuleFor(x => x.Numero).GreaterThan(0).When(x => x.Numero.HasValue);
    RuleFor(x => x.Ip).Matches(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$")
      .When(x => x.Ip != null)
      .WithMessage("El formato de la dirección IP no es válido. Debe ser IPv4 (ej. 192.168.1.1).");
    RuleFor(x => x.Tipo).MaximumLength(10).When(x => x.Tipo != null);
  }
}

public class MostradorSectorCrearDtoValidator : AbstractValidator<MostradorSectorCrearDto>
{
  public MostradorSectorCrearDtoValidator()
  {
    RuleFor(x => x.MostradorId).GreaterThan(0);
    RuleFor(x => x.SectorId).GreaterThan(0);
  }
}

public class RolPermisoCrearDtoValidator : AbstractValidator<RolPermisoCrearDto>
{
  public RolPermisoCrearDtoValidator()
  {
    RuleFor(x => x.RolId).GreaterThan(0);
    RuleFor(x => x.PermisoId).GreaterThan(0);
  }
}

public class SectorCrearDtoValidator : AbstractValidator<SectorCrearDto>
{
  public SectorCrearDtoValidator()
  {
    RuleFor(x => x.Letra)
      .MaximumLength(3)
      .Matches("^[A-Z]{1,3}$").When(x => x.Letra != null);
    RuleFor(x => x.Nombre).MaximumLength(50).When(x => x.Nombre != null);
    RuleFor(x => x.Descripcion).MaximumLength(120).When(x => x.Descripcion != null);
  }
}

public class SectorActualizarDtoValidator : AbstractValidator<SectorActualizarDto>
{
  public SectorActualizarDtoValidator()
  {
    RuleFor(x => x.Letra)
      .MaximumLength(3)
      .Matches("^[A-Z]{1,3}$").When(x => x.Letra != null);
    RuleFor(x => x.Nombre).MaximumLength(50).When(x => x.Nombre != null);
    RuleFor(x => x.Descripcion).MaximumLength(120).When(x => x.Descripcion != null);
  }
}

public class PuestoCrearDtoValidator : AbstractValidator<PuestoCrearDto>
{
  public PuestoCrearDtoValidator()
  {
    RuleFor(x => x.UsuarioId).GreaterThan(0);
    RuleFor(x => x.MostradorId).GreaterThan(0);
  }
}

public class PuestoActualizarDtoValidator : AbstractValidator<PuestoActualizarDto>
{
  public PuestoActualizarDtoValidator()
  {
    RuleFor(x => x.Login).NotEmpty().When(x => x.Login.HasValue);
    RuleFor(x => x.Logout).NotEmpty().When(x => x.Logout.HasValue);
  }
}

public class HistorialCrearDtoValidator : AbstractValidator<HistorialCrearDto>
{
  public HistorialCrearDtoValidator()
  {
    RuleFor(x => x.TicketId).GreaterThan(0UL);
    RuleFor(x => x.EstadoId).GreaterThan(0);
    RuleFor(x => x.Fecha).NotEmpty();
    RuleFor(x => x.Comentarios).MaximumLength(255).When(x => x.Comentarios != null);
  }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(x => x.Username).NotEmpty();
    RuleFor(x => x.Password).NotEmpty();
  }
}
