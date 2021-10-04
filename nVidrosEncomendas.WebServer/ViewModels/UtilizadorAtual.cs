using System;
using System.Collections.Generic;

namespace NVidrosEncomendas.WebServer.ViewModels
{
    public class UtilizadorAtual
    {
        public string Login { get; set; }
        public string NomeCompleto { get; set; }
        public bool Admin { get; set; }
        public bool AdminSav { get; set; }

        public bool Comercial { get; set; }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.IdNome> DepartamentosSav { get; set; }
        public DateTime DashboardDesde { get; set; }


        public UtilizadorAtual()
        {

        }
    }

    public class UtilizadorPerfil
    {
        public DateTime DashboardDesde { get; set; }

        public UtilizadorPerfil()
        {

        }

    }
}