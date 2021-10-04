using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class TipoAvarias
    {
        [Key]
        public int IdTipoAvaria { get; set; }

        public int NumTipoAvaria { get; set; }

        public string NomeTipoAvaria { get; set; }

        public bool Anulado { get; set; }

        public bool InfoExtra { get; set; }
    }
}