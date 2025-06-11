namespace TurneroApi.DTOs;

public class PuestoDto
{
    public uint Id { get; set; }
    public uint UsuarioId { get; set; }
    public uint MostradorId { get; set; }
    public DateTime? Login { get; set; }
    public DateTime? Logout { get; set; }
    public bool? Activo { get; set; }

    // DTOs de las relaciones para una respuesta más completa
    public MostradorDto MostradorNavigation { get; set; } = null!;
    public UsuarioDto UsuarioNavigation { get; set; } = null!;
}
