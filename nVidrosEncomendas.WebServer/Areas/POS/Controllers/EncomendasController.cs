﻿using Microsoft.Ajax.Utilities;
using NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas;
using NVidrosEncomendas.WebServer.BusinessLogic;
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    public class EncomendasController : ApiController
    {
        [HttpGet]
        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.Encomendas> TabelaEncomendas(string id)
        {
            DateTime dtMin = new DateTime(2019, 01, 01);
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
                          NomeSerie = c.SerieDoc.NomeSerie,
                          NumSerie = c.SerieDoc.NumSerie,
                          NumDoc = c.NumDoc,
                          NomeCliente = c.Cliente.NomeCliente,
                          NomeTipoEncomenda = c.TiposEncomenda.Select(x => x.TipoEncomendas.NomeTipoEncomenda).Aggregate((i,j) => i + ", "+ j),
                          RefObra = c.RefObra,
                          DataPedidoString = c.DataPedido.ToString(@"yyyy/MM/dd"),
                          SemanaEntrega = c.SemanaEntrega,
                          DataEntrega = c.DataEntrega,
                          DataExpedido = c.DataExpedido,
                          GuiaRemessa = c.GuiaRemessa,
                          Estado = c.Estado,
                          NumVidros = c.NumVidros,                         
                          EstadoRow = c.Estado == 2 && c.GuiaRemessa == "" ? 4 : c.Estado
                      };

            return res.OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
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

            res.VidroExtras = (from c in (new VidrosExtraBL(seriesBl.DbContext)).Lista()
                               select new ViewModels.IdNumNome
                               {
                                   Nome =  c.Nome + " (" + c.Num + ")",
                                  Id = c.Id,
                                  Num= c.Num
                               }).ToList();

            EncomendasBL encBL = new EncomendasBL(seriesBl.DbContext);
            KeyValuePair<Models.Encomendas, bool> BlRes = encBL.LerEncomenda(serie, numDoc);
            NVidrosEncomendas.WebServer.Models.Encomendas enc = BlRes.Key;
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
                                        TiposEncomenda = c.TiposDeEncomendaAssociados != null ? (from i in c.TiposDeEncomendaAssociados select i.IdTipoEncomenda).ToArray<int>(): new int[0]
                                    }).ToList();

            return res;
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
        public async Task<NVidrosEncomendas.WebServer.ViewModels.GerarRelatorioRes> RelatorioEncomendasPdf(ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParam)
        {
            EncomendasBL ecbl = new EncomendasBL();
            return await ecbl.GerarRelatorio(pesqParam);
        }
    }
}

