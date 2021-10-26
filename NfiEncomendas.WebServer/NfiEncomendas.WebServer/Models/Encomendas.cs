using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NfiEncomendas.WebServer.Models
{
    public class Encomendas
    {
        [Key]
        public int IdEncomenda { get; set; }

        public Series SerieDoc { get; set; }
        public int NumDoc { get; set; }

        public Clientes Cliente { get; set; }
        public TipoEncomendas TipoEncomenda { get; set; }

        public string NomeArtigo { get; set; }

        public string Cor { get; set; }

        public string Painel { get; set; }

        public string Producao { get; set; }

        public int SemanaEntrega { get; set; }

        public DateTime DataPedido { get; set; }
        public DateTime? DataAprovacao { get; set; }


        public DateTime DataExpedido { get; set; } //desativado por enquanto, 2014 11 23
        public string DataExpedidoString { get; set; }

        public string Fatura { get; set; }

        public string Notas { get; set; }

        public bool Anulada { get; set; }

        public int NumVaos { get; set; }
        public int Estado { get; set; }

        public List<EncomendasCompras> EncomendasCompras { get; set; }

        public string NumSerieEncomenda { get; set; }

        public bool ComprasOK { get; set; }

        public int? AnoEntrega { get; set; }

        public DateTime? DataProduzido { get; set; }




        public Encomendas()
        {
            DataPedido = DateTime.Now;
            Estado = 1;
            EncomendasCompras = new List<EncomendasCompras>();
        }


    }

}