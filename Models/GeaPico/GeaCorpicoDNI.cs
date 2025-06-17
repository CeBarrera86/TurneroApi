using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurneroApi.Models.GeaPico
{
    [Table("CLIENTE_DOCUMENTO", Schema = "dbo")]
    public class GeaCorpicoDNI
    {
        [Column("CLD_NUMERO_DOCUMENTO")]
        public string CLD_NUMERO_DOCUMENTO { get; set; } = null!;

        [Column("CLD_CLIENTE")]
        public int CLD_CLIENTE { get; set; }

        public GeaCorpicoCliente? ClienteGea { get; set; }
    }
}