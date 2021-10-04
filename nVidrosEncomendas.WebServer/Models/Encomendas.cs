using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NVidrosEncomendas.WebServer.Models
{
    public class Encomendas
    {
       
        [Key]
        public int IdEncomenda { get; set; }

        public Series SerieDoc { get; set; }
        public int NumDoc { get; set; }

        //public string NumSerieEncomenda { get; set; } 
        public Clientes Cliente { get; set; }
        //public TipoEncomendas TipoEncomenda { get; set; }

        public VidrosExtra VidrosExtra { get; set; }

        public string RefObra { get; set; }

        public string Producao { get; set; }

        public int SemanaEntrega { get; set; }

        public DateTime DataPedido { get; set; }
        public DateTime? DataProducao { get; set; }

        public DateTime? DataEntrega { get; set; }

        public string DataExpedido { get; set; }
        public string GuiaRemessa { get; set; }

        public string Notas { get; set; }

        public bool Anulada { get; set; }

        public int NumVidros { get; set; }
        public int Estado { get; set; }

        public virtual List<EncomendasTipoEncomenda> TiposEncomenda { get; set; }

        public Encomendas()
        {
            DataPedido = DateTime.Now;
            Estado = 1;
            
        }


    }
}