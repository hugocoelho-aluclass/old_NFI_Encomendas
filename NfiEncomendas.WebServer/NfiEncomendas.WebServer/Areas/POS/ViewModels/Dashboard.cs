using System;
using System.Collections.Generic;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels
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

        public int TotalComprasPendentes { get; set; }

        public int TotalEntreguesNaoFaturadas { get; set; }

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

        public string NomeArtigo { get; set; }



        public string Cor { get; set; }

        public string Painel { get; set; }
        public string Producao { get; set; }

        public int SemanaEntrega { get; set; }
        public DateTime DataPedido { get; set; }
        public string DataPedidoString { get; set; }
        public string DataExpedido { get; set; }

        public string Fatura { get; set; }
        public string Notas { get; set; }

        public bool Anulada { get; set; }

        public int NumVaos { get; set; }
        public int Estado { get; set; }

        public bool ComprasOK { get; set; }


    }

}