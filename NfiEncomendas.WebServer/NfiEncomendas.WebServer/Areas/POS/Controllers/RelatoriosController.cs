using NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas;
using NfiEncomendas.WebServer.BusinessLogic;
using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class RelatoriosController : ApiController
    {


        /// <summary>
        /// requeste para obter previamente tabelas necessárias para a realização do relatorio
        /// carrega a tabela series, para colocar no select form
        /// carrega a tabela com os tipo de encomendas para fazer as separação dos tipos de encomenda por setor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewModels.Relatorios.RelatorioDados RelatorioSemanaDados()
        {
            ViewModels.Relatorios.RelatorioDados res = new ViewModels.Relatorios.RelatorioDados();
            SeriesBL seriesBl = new SeriesBL();

            res.TipoEncomenda = (from c in (new TipoEncomendasBL(seriesBl.DbContext)).TipoEncomendasLista()
                                 select new ViewModels.Relatorios.TipoEncomenda
                                 {
                                     NomeTipoEncomenda = c.NomeTipoEncomenda + "  (" + c.NumTipoEncomenda + ")",
                                     NumTipoEncomenda = c.NumTipoEncomenda,
                                     SetorId = c.SetorEncomenda != null ? c.SetorEncomenda.IdSetorEncomenda : 0,
                                     SetorNome = c.SetorEncomenda != null ? c.SetorEncomenda.Nome : "",
                                 }).ToList();

            res.Series = (from s in seriesBl.SeriesLista()
                          select new ViewModels.Relatorios.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }
                          ).ToList();


            res.SetoresEncomenda = (from c in (new SetorEncomendasBL(seriesBl.DbContext)).SetorLista()
                                    select new ViewModels.Relatorios.SetorEncomenda
                                    {
                                        SetorId = c.IdSetorEncomenda,
                                        SetorNome = c.Nome,
                                        TiposEncomenda = c.TiposDeEncomendaAssociados != null ? (from i in c.TiposDeEncomendaAssociados select i.IdTipoEncomenda).ToArray<int>() : new int[0]
                                    }).ToList();

            return res;
        }


        /// <summary>
        /// Request para retornar um objecto com as tabelas necessárias para o relatório semanal
        /// recebe como parametro um objeto com duas variaveis, o ano e a semana
        /// </summary>
        /// <param name="pesqParams"></param>
        /// <returns></returns>
        [HttpPost]
        public ViewModels.Relatorios.TabelasRelatorioSemanal PesquisaRelatorioEncomendas(ViewModels.Relatorios.EncomendaPesquisaParamSemana pesqParams)
        {
            int ano = 0;
            Int32.TryParse(pesqParams.serie.NumSerie, out ano);
            //instancia um objeto da class EncomendasBL, para poder usar os metodos da classe
            EncomendasBL ebl = new EncomendasBL();
            // metodo para obter um objeto com o totais das encomendas
            ViewModels.Relatorios.RelatorioTotais totais = ebl.TotalEncomendas(pesqParams.semanaEntrega, ano);
            // metodo para obter uma lista dos tipos de encomendas e o total de encomendas por cliente
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Relatorios.RelatorioTipoEncomenda> lista = ebl.TotaisTipoEncomenda(pesqParams.semanaEntrega, ano);
            //juntar as duas variaveis obtidas num único objeto, que é enviado como resposta ao requeste para o front-end
            ViewModels.Relatorios.TabelasRelatorioSemanal tabelas = new ViewModels.Relatorios.TabelasRelatorioSemanal(lista.ToList(), totais);

            return tabelas;
        }


        /// <summary>
        /// Request para retornar um objecto com as tabelas necessárias para o relatório de várias semanas
        /// recebe como parametro um objeto com 3 variaveis, o ano e um intervalo de semanas
        /// </summary>
        /// <param name="pesqParams"></param>
        /// <returns>Retorna as tabelas com todos os totais das semanas escolhidas</returns>
        [HttpPost]
        public ViewModels.Relatorios.ListaTabelasRelatorioSemanal PesquisaRelatorioEncomendasAte(ViewModels.Relatorios.EncomendaPesquisaParamSemana pesqParams)
        {
            int it;
            //verifica a diferença de semanas e se tem alguma transição de ano
            if (pesqParams.ateSemanaEntrega > pesqParams.semanaEntrega)
            {
                it = pesqParams.ateSemanaEntrega - pesqParams.semanaEntrega;
            }
            else
            {
                it = 52 - pesqParams.semanaEntrega;
                it = it + pesqParams.ateSemanaEntrega;
            }

            //variaveis com ano e semana
            int semana = pesqParams.semanaEntrega;
            int ano = 0;
            Int32.TryParse(pesqParams.serie.NumSerie, out ano);

            //instancia objeto para usar as funções da classe
            EncomendasBL ebl = new EncomendasBL();

            // lista com as semanas todas
            List<ViewModels.Relatorios.TabelasRelatorioSemanal> l = new List<ViewModels.Relatorios.TabelasRelatorioSemanal>();


            //percorre todas as semanas, para obter as as quantidades totais de cada semana
            for (int i = 0; i <= it; i++)
            {
                //assim que ultrapassa a ultima semana do ano, inicia as semanas e incrementa para o próximo ano
                if (semana > 52)
                {
                    semana = 1;
                    ano++;
                }

                // Obtem  os totais da semana e ano enviados
                ViewModels.Relatorios.RelatorioTotais totais = ebl.TotalEncomendas(semana, ano);
                // os dados para as tabelas do relatório
                List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Relatorios.RelatorioTipoEncomenda> lista = ebl.TotaisTipoEncomenda(semana, ano);
                // passa os dados obtidos acima para um único objeto
                ViewModels.Relatorios.TabelasRelatorioSemanal tabelas = new ViewModels.Relatorios.TabelasRelatorioSemanal(lista.ToList(), totais, ano, semana);
                // e adiciona a uma lista com todas as semanas
                l.Add(tabelas);
                semana++;
            }

            ViewModels.Relatorios.ListaTabelasRelatorioSemanal listaTabelas = new ViewModels.Relatorios.ListaTabelasRelatorioSemanal(l);
            return listaTabelas;
        }


        [HttpPost]
        public List<ViewModels.Relatorios.RelatorioProdutoSav> PesquisaRelatorioSav(ViewModels.Relatorios.EncomendaPesquisaParamSemana pesqParams)
        {
    

            RelatoriosBL rbl = new RelatoriosBL();

            List<ViewModels.Relatorios.RelatorioProdutoSav> tabela = rbl.tabelaSav(pesqParams.serie.NumSerie, pesqParams.semanaEntrega);

            return tabela;


        }


    }
}