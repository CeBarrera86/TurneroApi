using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurneroApi.Models.GeaPico
{
    [Table("CLIENTE", Schema = "dbo")]
    public class GeaCorpicoCliente
    {
        [Key]
        [Column("CLI_ID")]
        public int CLI_ID { get; set; }

        [Column("CLI_TITULAR")]
        public string CLI_TITULAR { get; set; } = null!;
    }
}