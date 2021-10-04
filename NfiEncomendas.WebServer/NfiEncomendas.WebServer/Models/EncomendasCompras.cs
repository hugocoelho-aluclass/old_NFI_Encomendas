using System;
using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{
    public class EncomendasCompras
    {
        [Key]
        public int IdCompraEncomendas { get; set; }

        public string Material { get; set; }
        //public Encomendas Encomenda { get; set; }

        public string NotasFornecedor { get; set; }

        public int LinhaCompra { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataPedido { get; set; }


        //public virtual Encomendas Encomenda { get; set; }
        public EncomendasCompras()
        {

        }
    }
}