using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{

    public class ProdutoSav
    {
        [Key]
        public int IdProdutoSav { get; set; }

        public int NumProdutoSav { get; set; }

        public string NomeProdutoSav { get; set; }

        public bool Anulado { get; set; }
    }
}