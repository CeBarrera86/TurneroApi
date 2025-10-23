namespace TurneroApi.DTOs;

public class SectorDto
{
    public uint Id { get; set; }
    public uint? PadreId { get; set; }
    public string? Letra { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    
    public SectorDto? Padre { get; set; }
}
