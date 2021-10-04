using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Setor
    {
        [Key]
        public int IdSetor { get; set; }

        public int NumSetor { get; set; }

        public string Nome { get; set; }

        public bool Anulado { get; set; }
    }
}