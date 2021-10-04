using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NVidrosEncomendas.WebServer.Models
{
    public class EncomendasTipoEncomenda
    {

        [Key, Column(Order = 0)]
        public int EncomendaId { get; set; }

        [Key, Column(Order = 1)]
        public int TipoEncomendaId { get; set; }

        [ForeignKey("EncomendaId")]
        public virtual Encomendas Encomenda { get; set; }

        [ForeignKey("TipoEncomendaId")]
        public TipoEncomendas  TipoEncomendas { get; set; }

    }
}