using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{
    public class EstadoRecolha
    {
        [Key]
        public int Id { get; set; }

        public string NomeEstado { get; set; }
        public string Cor { get; set; } = "#ffffff";

        public bool EstadoFechaRecolha { get; set; } = false;
        public bool Anulado { get; set; }


    }
}