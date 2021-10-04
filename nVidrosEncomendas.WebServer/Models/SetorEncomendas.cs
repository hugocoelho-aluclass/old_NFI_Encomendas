using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class SetorEncomendas
    {
        [Key]
        public int IdSetorEncomenda { get; set; }

        public int NumSetor { get; set; }

        public string Nome { get; set; }

        public bool Anulado { get; set; }

        public List<TipoEncomendas> TiposDeEncomendaAssociados { get; set; }

    }
}