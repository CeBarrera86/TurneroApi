namespace TurneroApi.DTOs;

public class PuestoDto
{
    public uint Id { get; set; }
    public uint UsuarioId { get; set; }
    public uint MostradorId { get; set; }
    public DateTime? Login { get; set; }
    public DateTime? Logout { get; set; }
    public bool? Activo { get; set; }

    public string? UsuarioNombre { get; set; }
    public uint? MostradorNumero { get; set; }
}
