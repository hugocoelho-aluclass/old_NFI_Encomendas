using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels
{
    public class Relatorios
    {

        public class Serie
        {
            public string NumSerie { get; set; }
            public int UltDoc { get; set; }
            public bool SerieDefeito { get; set; }
        }

        public class TipoEncomenda
        {
            public int NumTipoEncomenda { get; set; }
            public string NomeTipoEncomenda { get; set; }
            public int SetorId { get; set; }
            public string SetorNome { get; set; }

        }

        public class SetorEncomenda
        {
            public int SetorId { get; set; }
            public string SetorNome { get; set; }

            public int[] TiposEncomenda { get; set; }
        }


        public class RelatorioDados
        {

            public List<TipoEncomenda> TipoEncomenda { get; set; }

            public List<Serie> Series { get; set; }
            public List<SetorEncomenda> SetoresEncomenda { get; set; }


            public RelatorioDados()
            {

            }
        }

        public class EncomendaPesquisaParamSemana
        {
            public int semanaEntrega { get; set; }

            public Serie serie { get; set; }

            public bool semanaEntregaAteBool { get; set; }

            public int ateSemanaEntrega { get; set; }

            public string TipoRetorno { get; set; }

            public EncomendaPesquisaParamSemana()
            {
                TipoRetorno = "json";
            }
        }

        //interface para receber tabelas com os dados para o relatorio semanal web
        public class RelatorioEncTipoEncomenda
        {
            public int NumTipoEncomenda { get; set; }
            public string NomeTipoEncomenda { get; set; }
            public int SetorEncomenda_IdSetorEncomenda { get; set; }
            public int totalPrix { get; set; }
            public int totalWis { get; set; }
            public int totalResto { get; set; }
            public int total { get; set; }
            public int totalPrixProd { get; set; }
            public int totalWisProd { get; set; }
            public int totalRestoProd { get; set; }
            public int totalProd { get; set; }


            public RelatorioEncTipoEncomenda()
            {
            }

        }

        //interface para receber tabelas com os dados para o relatorio Excel
        public class RelatorioExcelTipoEncomenda
        {
            public int IdTipoEncomenda { get; set; }
            public int NumTipoEncomenda { get; set; }
            public string NomeTipoEncomenda { get; set; }
            public int SetorEncomenda_IdSetorEncomenda { get; set; }
            public int totalPrix { get; set; }
            public int totalWis { get; set; }
            public int totalResto { get; set; }
            public int totalConcluidoPrix { get; set; }
            public int totalConcluidoWis { get; set; }
            public int totalConcluidoResto { get; set; }
            public int totalProdPrix { get; set; }
            public int totalProdWis { get; set; }
            public int totalProdResto { get; set; }


            public RelatorioExcelTipoEncomenda()
            {
            }

        }


        //interface para receber tabelas com os dados para o relatorio Web
        public class RelatorioTipoEncomenda
        {
            public int IdTipoEncomenda { get; set; }
            public int NumTipoEncomenda { get; set; }
            public string NomeTipoEncomenda { get; set; }
            public int SetorEncomenda_IdSetorEncomenda { get; set; }
            public int capacidadePrix { get; set; }
            public int capacidadeWis { get; set; }
            public int capacidadeResto { get; set; }
            public int totalPrix { get; set; }
            public int totalWis { get; set; }
            public int totalResto { get; set; }
            public int totalConcluidoPrix { get; set; }
            public int totalConcluidoWis { get; set; }
            public int totalConcluidoResto { get; set; }
            public int totalProdPrix { get; set; }
            public int totalProdWis { get; set; }
            public int totalProdResto { get; set; }
            public int totalAtrasadoPrix { get; set; }
            public int totalAtrasadoWis { get; set; }
            public int totalAtrasadoResto { get; set; }



            public RelatorioTipoEncomenda()
            {
            }

        }

        public class RelatorioExcelTotais
        {
            public int totalPrix { get; set; }
            public int totalWis { get; set; }
            public int totalResto { get; set; }
            public int totalConcluidoPrix { get; set; }
            public int totalConcluidoWis { get; set; }
            public int totalConcluidoResto { get; set; }
            public int totalProdPrix { get; set; }
            public int totalProdWis { get; set; }
            public int totalProdResto { get; set; }


            public RelatorioExcelTotais()
            {
            }

        }


        public class RelatorioTotais
        {
            public int totalPrix { get; set; }
            public int totalWis { get; set; }
            public int totalResto { get; set; }
            public int totalConcluidoPrix { get; set; }
            public int totalConcluidoWis { get; set; }
            public int totalConcluidoResto { get; set; }
            public int totalProdPrix { get; set; }
            public int totalProdWis { get; set; }
            public int totalProdResto { get; set; }

            public int totalAtrasadoPrix { get; set; }
            public int totalAtrasadoWis { get; set; }
            public int totalAtrasadoResto { get; set; }


            public RelatorioTotais()
            {
            }

        }


        //interface para receber tabelas com os dados para o relatorio Web
        public class RelatorioProdutoSav
        {
            public int idProdutoSav { get; set; }
            public string nomeProdutoSav { get; set; }
            public int total { get; set; }
            public int totalResolucao { get; set; }
            public int totalEnvio { get; set; }
            public int TotalAguardaResp { get; set; }
            public int totalRecusado { get; set; }
            public int totalFechado { get; set; }
            public int totalRecolha { get; set; }
        
            public RelatorioProdutoSav()
            {
            }

        }

        public class RelatorioEncTotais
        {
            public int totalPrix { get; set; }
            public int totalWis { get; set; }
            public int totalResto { get; set; }
            public int total { get; set; }
            public int totalPrixProd { get; set; }
            public int totalWisProd { get; set; }
            public int totalRestoProd { get; set; }
            public int totalProd { get; set; }


            public RelatorioEncTotais()
            {
            }

        }


        public class TabelasRelatorioSemanal
        {

            public List<RelatorioTipoEncomenda> tabelaTipoEncomenda { get; set; }

            public RelatorioTotais totaisTabela { get; set; }

            public int? ano { get; set; }

            public int? semana { get; set; }

            public TabelasRelatorioSemanal()
            {
            }

            public TabelasRelatorioSemanal(List<RelatorioTipoEncomenda> l, RelatorioTotais t)
            {
                tabelaTipoEncomenda = l;
                totaisTabela = t;
            }

            public TabelasRelatorioSemanal(List<RelatorioTipoEncomenda> l, RelatorioTotais t, int a, int s)
            {

                tabelaTipoEncomenda = l;
                totaisTabela = t;
                ano = a;
                semana = s;
            }
        }


        public class ListaTabelasRelatorioSemanal
        {

            public List<TabelasRelatorioSemanal> listaTabelasRelatorio { get; set; }

            public ListaTabelasRelatorioSemanal()
            {
            }

            public ListaTabelasRelatorioSemanal(List<TabelasRelatorioSemanal> l)
            {
                listaTabelasRelatorio = l;
            }
        }

    }
}