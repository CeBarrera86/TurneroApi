using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurneroApi.Models.GeaPico;

[Table("USUARIOS", Schema = "dbo")]
public class GeaSeguridad
{
    [Key]
    [Column("USU_CODIGO")]
    public string USU_CODIGO { get; set; } = null!;

    [Column("USU_PASSWORD")]
    public string USU_PASSWORD { get; set; } = null!;
}