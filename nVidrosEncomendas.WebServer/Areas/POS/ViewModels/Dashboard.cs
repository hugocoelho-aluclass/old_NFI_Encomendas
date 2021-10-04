using System;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.Areas.POS.ViewModels
{
    public class Dashboard
    {
        public int TotalEncomendas { get; set; }
        public int TotalPendentes { get; set; }
        public int TotalProducao { get; set; }
        public int TotalExpedidas { get; set; }

        public int TotalCaixilhos { get; set; }
        public int TotalPortoesAp { get; set; }

        public int TotalPortoesSold { get; set; }

        public int TotalEstores { get; set; }

        public int TotalEncSemSemana { get; set; }
        public int TotalEncAtrasada { get; set; }
        public int TotalEncEstaSemana { get; set; }
        public int TotalEncProxSemana1 { get; set; }
        public int TotalEncProxSemana2 { get; set; }
        public int TotalEncProxSemana3 { get; set; }
        public int TotalEncProxSemana4mais { get; set; }

      
        public int TotalEntreguesNaoGuiaRemessadas { get; set; }

        public List<EncomendaDashboard> EncomendasDashboard { get; set; }
        public List<EncomendaDashboard> EncomendasAtrasadas { get; set; }
        public List<EncomendaDashboard> EncomendasSemEntrega { get; set; }

        public List<EncomendaDashboard> EncomendasSemana0 { get; set; }
        public List<EncomendaDashboard> EncomendasSemana1 { get; set; }
        public List<EncomendaDashboard> EncomendasSemana2 { get; set; }
        public List<EncomendaDashboard> EncomendasSemana3 { get; set; }
        public List<EncomendaDashboard> EncomendasSemana4 { get; set; }
        public List<EncomendaDashboard> EncomendasSemana5Mais { get; set; }

        public Dashboard()
        {

        }

    }

    public class DashboardReq
    {
        public DateTime dataDesde { get; set; }
        public int tipo { get; set; }

        public DateTime GetData()
        {
            return new DateTime(dataDesde.Year, dataDesde.Month, dataDesde.Day, 0, 0, 0);

        }
    }

    public class DashboardEncomendas
    {
        public List<EncomendaDashboard> Encomendas { get; set; }

    }

    public class EncomendaDashboard
    {
        public int IdEncomenda { get; set; }
        public string NomeSerie { get; set; }
        public int NumDoc { get; set; }

        public string NomeCliente { get; set; }
        public int NumCliente { get; set; }

        public int NumTipoEncomenda { get; set; }
        public string NomeTipoEncomenda { get; set; }

        public string RefObra { get; set; }

        public string Producao { get; set; }

        public int SemanaEntrega { get; set; }
        public DateTime DataPedido { get; set; }
        public string DataPedidoString { get; set; }
        public string DataExpedido { get; set; }

        public string GuiaRemessa { get; set; }
        public string Notas { get; set; }

        public bool Anulada { get; set; }

        public int NumVidros { get; set; }
        public int Estado { get; set; }
        public EncomendaDashboard()
        {

        }
        public EncomendaDashboard(Models.Encomendas c)
        {
            IdEncomenda = c.IdEncomenda;
            NomeSerie = c.SerieDoc.NumSerie;
            NumDoc = c.NumDoc;
            NomeCliente = c.Cliente.NomeCliente;
            NomeTipoEncomenda = c.TiposEncomenda.Select(x => x.TipoEncomendas.NomeTipoEncomenda).Aggregate((i,j) => i + ", " + j);
            RefObra = c.RefObra;
            DataPedidoString = c.DataPedido.ToString(@"yyyy/MM/dd");
            SemanaEntrega = c.SemanaEntrega;
            GuiaRemessa = c.GuiaRemessa;
            Estado = c.Estado;
            NumVidros = c.NumVidros;
        }

    }

}