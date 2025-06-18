using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurneroApi.Models.GeaPico
{
    [Table("CLIENTE_DOCUMENTO", Schema = "dbo")]
    public class GeaCorpicoDNI
    {
        [Column("CLD_EMPRESA")]
        public short CLD_EMPRESA { get; set; }

        [Column("CLD_CLIENTE")]
        public int CLD_CLIENTE { get; set; }

        [Column("CLD_TIPO_DOCUMENTO")]
        public byte CLD_TIPO_DOCUMENTO { get; set; }

        [Column("CLD_NUMERO_DOCUMENTO")]
        public string CLD_NUMERO_DOCUMENTO { get; set; } = null!;

        // Cargar los datos del cliente asociado a este documento.
        public GeaCorpicoCliente? ClienteGea { get; set; }
    }
}