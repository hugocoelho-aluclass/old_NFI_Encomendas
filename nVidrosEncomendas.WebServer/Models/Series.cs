using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Series
    {
        [Key]
        public string NumSerie { get; set; }

        public string NomeSerie { get; set; }

        public bool Inativa { get; set; }

        public bool SerieDefeito { get; set; }

        public int UltimoDoc { get; set; }
        public int UltimoDocSav { get; set; }

        public Series()
        {
            UltimoDoc = 0;
        }
    }
}