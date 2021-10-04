using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Clientes
    {
        [Key]
        public int IdCliente { get; set; }

        public int NumCliente { get; set; }
        public string NomeCliente { get; set; }

        public bool Anulado { get; set; }


        public Clientes()
        {

        }
    }
}