using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class TipoEncomendas
    {
        [Key]
        public int IdTipoEncomenda { get; set; }

        public int NumTipoEncomenda { get; set; }

        public string NomeTipoEncomenda { get; set; }

        public SetorEncomendas SetorEncomenda { get; set; }

        public bool Anulado { get; set; }

    }
}