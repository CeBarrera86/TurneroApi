using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurneroApi.Models.GeaPico
{
    [Table("CLIENTE", Schema = "dbo")]
    public class GeaCorpicoCliente
    {
        [Column("CLI_EMPRESA")]
        public short CLI_EMPRESA { get; set; }

        [Column("CLI_ID")]
        public int CLI_ID { get; set; }

        [Column("CLI_TITULAR")]
        public string? CLI_TITULAR { get; set; }
    }
}