using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class VidrosExtra
    {
        [Key]
        public int Id { get; set; }

        public int Num { get; set; }

        public string Nome { get; set; }

        public bool Anulado { get; set; }
    }
}