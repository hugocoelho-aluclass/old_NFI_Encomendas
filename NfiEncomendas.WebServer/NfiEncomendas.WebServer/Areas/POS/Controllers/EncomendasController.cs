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
    public class EncomendasController : ApiController
    {

        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.Encomendas> TabelaEncomendas(string id)
        {
            DateTime dtMin = new DateTime(2010, 01, 01);
            if (id != "")
            {
                string[] dataSplited = id.Split('-');
                dtMin = new DateTime(Int32.Parse(dataSplited[0]), Int32.Parse(dataSplited[1]), Int32.Parse(dataSplited[2]));
            }
            var res = from c in EncomendasCache.GetEncomendasLista()
                      where c.DataPedido > dtMin
                      select new ViewModels.Encomendas.Encomendas()
                      {
                          IdEncomenda = c.IdEncomenda,
                          NomeSerie = c.SerieDoc.NumSerie,
                          NumDoc = c.NumDoc,
                          NomeCliente = c.Cliente.NomeCliente,
                          NomeTipoEncomenda = c.TipoEncomenda.NomeTipoEncomenda,
                          NomeArtigo = c.NomeArtigo,
                          Cor = c.Cor,
                          Painel = c.Painel,
                          DataPedidoString = c.DataPedido.ToString(@"yyyy/MM/dd"),
                          DataAprovacaoString = c.DataAprovacao.HasValue ? c.DataAprovacao.Value.ToString(@"yyyy/MM/dd") : "",
                          SemanaEntrega = c.SemanaEntrega,
                          Fatura = c.Fatura,
                          Estado = c.Estado,
                          NumVaos = c.NumVaos,
                          ComprasOK = (c.Estado <= 1) ? c.ComprasOK : true,
                          NumSerie = c.NumSerieEncomenda,
                          EstadoRow = c.Estado == 2 && c.Fatura == "" ? 4 : c.Estado
                      };

            return res.OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        public string AtualizaComprasEncomendas()
        {
            EncomendasBL encomendas = new EncomendasBL();
            encomendas.AtualizaComprasEncomendas();
            return "encomendas atualizadas!";
        }

        [HttpGet]
        public ViewModels.Encomendas.PagEditEncomenda EditEncomenda(string serie, int numDoc)
        {
            SessionObject sj = SessionObject.GetMySessionObject(System.Web.HttpContext.Current);

            if (!PermissoesBL.CheckPermissao(sj.OperadorObject, PermissoesBL.Permissoes.Encomendas_Ver))
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "" };
                throw new HttpResponseException(msg);
            }

            ViewModels.Encomendas.PagEditEncomenda res = new ViewModels.Encomendas.PagEditEncomenda();
            SeriesBL seriesBl = new SeriesBL();
            res.Encomenda = new ViewModels.Encomendas.Encomendas();
            res.Series = (from s in seriesBl.SeriesLista()
                          select new ViewModels.Encomendas.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }).ToList();
            res.Clientes = (from c in (new ClientesBL(seriesBl.DbContext)).ClientesListaOrdemNome()
                            select new ViewModels.Encomendas.Cliente
                            {
                                NomeCliente = c.NomeCliente + "  (" + c.NumCliente + ")",
                                NumCliente = c.NumCliente
                            }).ToList();

            res.TipoEncomenda = (from c in (new TipoEncomendasBL(seriesBl.DbContext)).TipoEncomendasLista()
                                 select new ViewModels.Encomendas.TipoEncomenda
                                 {
                                     NomeTipoEncomenda = c.NomeTipoEncomenda + "  (" + c.NumTipoEncomenda + ")",
                                     NumTipoEncomenda = c.NumTipoEncomenda
                                 }).ToList();

            EncomendasBL encBL = new EncomendasBL(seriesBl.DbContext);
            KeyValuePair<Models.Encomendas, bool> BlRes = encBL.LerEncomenda(serie, numDoc);
            NfiEncomendas.WebServer.Models.Encomendas enc = BlRes.Key;
            res.Encomenda = new ViewModels.Encomendas.Encomendas();
            res.Encomenda.EncomendaToVM(enc);
            res.Encomenda.NovaEncomenda = BlRes.Value;


            return res;
        }

        [HttpPost]
        public string AtualizaEncomenda(ViewModels.Encomendas.Encomendas encomenda)
        {
            SessionObject sj = SessionObject.GetMySessionObject(System.Web.HttpContext.Current);
            if (!PermissoesBL.CheckPermissao(sj.OperadorObject, PermissoesBL.Permissoes.Encomendas_Editar))
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "" };
                throw new HttpResponseException(msg);

            }
            EncomendasBL encBl = new EncomendasBL();
            Models.Encomendas c = encomenda.ToModel(encBl.DbContext);
            encBl.AtualizaEncomenda(c);
            return "sucesso";
        }

        [HttpGet]
        public ViewModels.Encomendas.PagEncomendaPesquisa PesquisaEncomenda()
        {
            ViewModels.Encomendas.PagEncomendaPesquisa res = new ViewModels.Encomendas.PagEncomendaPesquisa();
            SeriesBL seriesBl = new SeriesBL();

            res.Clientes = (from c in (new ClientesBL(seriesBl.DbContext)).ClientesListaOrdemNome()
                            select new ViewModels.Encomendas.Cliente
                            {
                                NomeCliente = c.NomeCliente + "  (" + c.NumCliente + ")",
                                NumCliente = c.NumCliente
                            }).ToList();

            res.TipoEncomenda = (from c in (new TipoEncomendasBL(seriesBl.DbContext)).TipoEncomendasLista()
                                 select new ViewModels.Encomendas.TipoEncomenda
                                 {
                                     NomeTipoEncomenda = c.NomeTipoEncomenda + "  (" + c.NumTipoEncomenda + ")",
                                     NumTipoEncomenda = c.NumTipoEncomenda,
                                     SetorId = c.SetorEncomenda != null ? c.SetorEncomenda.IdSetorEncomenda : 0,
                                     SetorNome = c.SetorEncomenda != null ? c.SetorEncomenda.Nome : "",
                                 }).ToList();

            res.Series = (from s in seriesBl.SeriesLista()
                          select new ViewModels.Encomendas.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }
                          ).ToList();


            res.SetoresEncomenda = (from c in (new SetorEncomendasBL(seriesBl.DbContext)).SetorLista()
                                    select new ViewModels.Encomendas.SetorEncomenda
                                    {
                                        SetorId = c.IdSetorEncomenda,
                                        SetorNome = c.Nome,
                                        TiposEncomenda = c.TiposDeEncomendaAssociados != null ?(from i in c.TiposDeEncomendaAssociados select i.IdTipoEncomenda).ToArray<int>() : new int[0]
                                    }).ToList();

            return res;
        }


        /// <summary>
        /// requeste para obter previamente tabelas necessárias para a realização do relatorio
        /// carrega a tabela series, para colocar no select form
        /// carrega a tabela com os tipo de encomendas para fazer as separação dos tipos de encomenda por setor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ViewModels.Encomendas.RelatorioDados RelatorioSemanaDados()
        {
            ViewModels.Encomendas.RelatorioDados res = new ViewModels.Encomendas.RelatorioDados();
            SeriesBL seriesBl = new SeriesBL();

            res.TipoEncomenda = (from c in (new TipoEncomendasBL(seriesBl.DbContext)).TipoEncomendasLista()
                                 select new ViewModels.Encomendas.TipoEncomenda
                                 {
                                     NomeTipoEncomenda = c.NomeTipoEncomenda + "  (" + c.NumTipoEncomenda + ")",
                                     NumTipoEncomenda = c.NumTipoEncomenda,
                                     SetorId = c.SetorEncomenda != null ? c.SetorEncomenda.IdSetorEncomenda : 0,
                                     SetorNome = c.SetorEncomenda != null ? c.SetorEncomenda.Nome : "",
                                 }).ToList();

            res.Series = (from s in seriesBl.SeriesLista()
                          select new ViewModels.Encomendas.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }
                          ).ToList();


            res.SetoresEncomenda = (from c in (new SetorEncomendasBL(seriesBl.DbContext)).SetorLista()
                                    select new ViewModels.Encomendas.SetorEncomenda
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
        public TabelasRelatorioSemanal PesquisaRelatorioEncomendas(ViewModels.Encomendas.EncomendaPesquisaParamSemana pesqParams)
        {
            //instancia um objeto da class EncomendasBL, para poder usar os metodos da classe
            EncomendasBL ebl = new EncomendasBL();
            // metodo para obter um objeto com o totais das encomendas
            RelatorioEncTotais totais = ebl.TotalEncomendas(pesqParams.semanaEntrega, pesqParams.serie.NumSerie);
            // metodo para obter uma lista dos tipos de encomendas e o total de encomendas por cliente
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioEncTipoEncomenda> lista = ebl.TotaisTipoEncomenda(pesqParams.semanaEntrega, pesqParams.serie.NumSerie);
            //juntar as duas variaveis obtidas num único objeto, que é enviado como resposta ao requeste para o front-end
            TabelasRelatorioSemanal tabelas = new TabelasRelatorioSemanal(lista.ToList(), totais);

            return tabelas;
        }






        [HttpPost]
        public PagEncomendaPesquisaRes PesquisaEncomendaRes(ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
            EncomendasBL encbl = new EncomendasBL();
            return encbl.PesqParamRes(pesqParams);
        }

        public PagEncomendaPesquisaProd PesquisaEncomendaProd(ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
            EncomendasBL encbl = new EncomendasBL();

            return encbl.PesqProdParamRes(pesqParams);
        }

        public PagEncomendaPesquisaAdmin PesquisaEncomendaAdmin(ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
            EncomendasBL encbl = new EncomendasBL();
            return encbl.PesqAdminParamRes(pesqParams);
        }

        [HttpPost]
        public async Task<NfiEncomendas.WebServer.ViewModels.GerarRelatorioRes> RelatorioEncomendasPdf(ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParam)
        {
            EncomendasBL ecbl = new EncomendasBL();
            return await ecbl.GerarRelatorio(pesqParam);
        }
    }
}

