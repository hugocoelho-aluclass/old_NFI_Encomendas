
using jsreport.Client;
using jsreport.Client.Entities;
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class EncomendasBL
    {
        public KeyValuePair<Models.Encomendas, bool> LerEncomendaResultado = new KeyValuePair<Encomendas, bool>();

        private AppDbContext _dbContext;
        public AppDbContext DbContext
        {
            get
            {
                if (_dbContext == null) _dbContext = new AppDbContext();
                return _dbContext;
            }
            set { _dbContext = value; }
        }

        public EncomendasBL()
        {
        }

        public EncomendasBL(AppDbContext db)
        {
            DbContext = db;
        }
        public IEnumerable<Encomendas> EncomendasListaAnulados()
        {
            return DbContext.Encomendas.AsParallel().OrderBy(x => x.SerieDoc.NumSerie).ThenBy(x => x.NumDoc);
        }

        public IEnumerable<Encomendas> EncomendasLista()
        {
            return DbContext.Encomendas
                .Include(x => x.SerieDoc)
                .Include(x => x.Cliente)
                .Include(x => x.TiposEncomenda)
                .Include(x => x.TiposEncomenda.Select(z => z.TipoEncomendas))
                .Where(x => x.Anulada == false)
                .OrderBy(x => x.SerieDoc.NumSerie).ThenBy(x => x.NumDoc);
        }

        public KeyValuePair<Models.Encomendas, bool> LerEncomenda(int id)
        {
            var res = DbContext.Encomendas
                .Include(x => x.Cliente)
                .Include(x => x.SerieDoc)
                .Include(x => x.TiposEncomenda)
                .Include(x => x.TiposEncomenda.Select(z => z.TipoEncomendas))
                .Where(x => x.IdEncomenda == id).FirstOrDefault();

            bool nova = res == null;
            if (res == null)
            {
                res = new Encomendas();

                SeriesBL sbl = new SeriesBL(DbContext);
                res.SerieDoc = sbl.SerieDefeito();
                res.NumDoc = res.SerieDoc.UltimoDoc + 1;
            }

            return new KeyValuePair<Models.Encomendas, bool>(res, nova); ;
        }

        public KeyValuePair<Models.Encomendas, bool> LerEncomenda(string serie, int numDoc)
        {
            var res = DbContext.Encomendas
                 .Include(x => x.Cliente)
                .Include(x => x.TiposEncomenda)
                .Include(x => x.TiposEncomenda.Select(z => z.TipoEncomendas))
                .Where(x => x.SerieDoc.NumSerie == serie && x.NumDoc == numDoc).FirstOrDefault();

            bool nova = res == null;
            if (res == null)
            {
                res = new Encomendas();
                SeriesBL sbl = new SeriesBL(DbContext);
                res.SerieDoc = sbl.ProcuraSerieOuDefeito(serie);
                res.NumDoc = res.SerieDoc.UltimoDoc + 1;
            }
            return new KeyValuePair<Models.Encomendas, bool>(res, nova);
        }

        public void AtualizaEncomenda(Encomendas enc)
        {
            try
            {
                Clientes encCl = enc.Cliente;
                //TipoEncomendas tcCl = enc.TipoEncomenda;

                Encomendas _sr = DbContext.Encomendas
                    .Include(x => x.SerieDoc)
                    .Include(x => x.Cliente)
                    //.Include(x => x.TipoEncomenda)
                    .Include(x => x.TiposEncomenda)
                    .Include(x => x.TiposEncomenda.Select(z => z.TipoEncomendas))
                    .Where(x => x.IdEncomenda == enc.IdEncomenda && x.NumDoc == enc.NumDoc && x.SerieDoc.NumSerie == enc.SerieDoc.NumSerie).FirstOrDefault();

                bool novo = false;
                if (_sr == null)
                {
                    novo = true;
                    _sr = new Encomendas();
                    _sr.NumDoc = enc.SerieDoc.UltimoDoc + 1;
                }
                else
                {
                    _sr.NumDoc = enc.NumDoc;
                }

                _sr.SerieDoc = enc.SerieDoc;
                _sr.Cliente = encCl;
                _sr.RefObra = enc.RefObra;
                _sr.VidrosExtra = enc.VidrosExtra;
                _sr.Producao = enc.Producao;
                _sr.DataPedido = enc.DataPedido;
                _sr.DataExpedido = enc.DataExpedido;

                _sr.DataEntrega = enc.DataEntrega;
                _sr.SemanaEntrega = enc.SemanaEntrega;
                _sr.GuiaRemessa = enc.GuiaRemessa;
                _sr.Notas = enc.Notas;
                _sr.Anulada = enc.Anulada;
                _sr.NumVidros = enc.NumVidros;
                _sr.Estado = enc.Estado;
                _sr.TiposEncomenda = enc.TiposEncomenda.Select(x => new EncomendasTipoEncomenda()
                {
                    Encomenda = _sr,
                    TipoEncomendaId = x.TipoEncomendaId
                }).ToList();

                if (novo)
                {
                    _sr.NumDoc = enc.SerieDoc.UltimoDoc + 1;
                    _sr.SerieDoc.UltimoDoc = _sr.NumDoc;
                    DbContext.Encomendas.Add(_sr);
                }
                EncomendasCache.UpdateCache = true;
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                throw ex;
            }
        }


        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> OrdernarLista(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> lista)
        {
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> resLista = lista;
            if (pesqParams.Ordenacao == 0)
            {
                resLista = resLista.OrderBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.Ordenacao == 1)
            {
                //Semana Entrega
                resLista = resLista.OrderBy(x => x.SemanaEntrega).ToList();

            }
            else if (pesqParams.Ordenacao == 2)
            {
                //Cliente
                resLista = resLista.OrderBy(x => x.NomeCliente).ToList();
            }
            else if (pesqParams.Ordenacao == 3)
            {
                //Cliente (Agrupar)
                resLista = resLista.OrderBy(x => x.NomeCliente).ToList();
                string lastNomeCliente = "";
                int listaCountFixo = resLista.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (resLista[i].NomeCliente != lastNomeCliente)
                    {
                        resLista.Add(new Areas.POS.ViewModels.Encomendas.EncomendasLinha2() { NomeCliente = resLista[i].NomeCliente, NomeSerie = "0", NumDoc = 0, Separador = true });
                    }
                    lastNomeCliente = resLista[i].NomeCliente;
                }

                resLista = resLista.OrderBy(x => x.NomeCliente).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.Ordenacao == 4 || pesqParams.Ordenacao == 6)
            {
                //Tipo de obra
                if (pesqParams.Ordenacao == 4)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ToList();

                }
                else if (pesqParams.Ordenacao == 6)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.SemanaEntrega).ThenBy(x => x.NumDoc).ToList();
                }
            }
            else if (pesqParams.Ordenacao == 5 || pesqParams.Ordenacao == 7)
            {
                //Tipo de obra (Agrupar)
                resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ToList();
                string lastNomeTipoEncomenda = "";
                int listaCountFixo = resLista.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (resLista[i].NomeTipoEncomenda != lastNomeTipoEncomenda)
                    {
                        resLista.Add(new Areas.POS.ViewModels.Encomendas.EncomendasLinha2() { NomeCliente = resLista[i].NomeCliente, NomeTipoEncomenda = resLista[i].NomeTipoEncomenda, NomeSerie = "0", NumDoc = 0, Separador = true });
                    }
                    lastNomeTipoEncomenda = resLista[i].NomeTipoEncomenda;
                }
                if (pesqParams.Ordenacao == 5)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();


                }
                else if (pesqParams.Ordenacao == 7)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.SemanaEntrega).ThenBy(x => x.NumDoc).ToList();
                }
            }

            else
            {
                resLista = resLista.OrderBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            return resLista;
        }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> OrdernarLista(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
       List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> lista)
        {
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> resLista = lista;
            if (pesqParams.Ordenacao == 0)
            {
                resLista = resLista.OrderBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.Ordenacao == 1)
            {
                //Semana Entrega
                resLista = resLista.OrderBy(x => x.SemanaEntrega).ToList();

            }
            else if (pesqParams.Ordenacao == 2)
            {
                resLista = resLista.OrderBy(x => x.NomeCliente).ToList();
            }
            else if (pesqParams.Ordenacao == 3)
            {
                //Cliente (Agrupar)
                resLista = resLista.OrderBy(x => x.NomeCliente).ToList();
                string lastNomeCliente = "";
                int listaCountFixo = resLista.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (resLista[i].NomeCliente != lastNomeCliente)
                    {
                        resLista.Add(new Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd() { NomeCliente = resLista[i].NomeCliente, NomeSerie = "0", NumDoc = 0, Separador = true });
                    }
                    lastNomeCliente = resLista[i].NomeCliente;
                }

                resLista = resLista.OrderBy(x => x.NomeCliente).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.Ordenacao == 4 || pesqParams.Ordenacao == 6)
            {
                //Tipo de obra
                if (pesqParams.Ordenacao == 4)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ToList();

                }
                else if (pesqParams.Ordenacao == 6)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.SemanaEntrega).ThenBy(x => x.NumDoc).ToList();
                }

            }
            else if (pesqParams.Ordenacao == 5 || pesqParams.Ordenacao == 7)
            {
                //Tipo de obra (Agrupar)
                resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ToList();
                string lastNomeTipoEncomenda = "";
                int listaCountFixo = resLista.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (resLista[i].NomeTipoEncomenda != lastNomeTipoEncomenda)
                    {
                        resLista.Add(new Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd() { NomeCliente = resLista[i].NomeCliente, NomeTipoEncomenda = resLista[i].NomeTipoEncomenda, NomeSerie = "0", NumDoc = 0, Separador = true, SemanaEntrega = 0 });
                    }
                    lastNomeTipoEncomenda = resLista[i].NomeTipoEncomenda;
                }
                if (pesqParams.Ordenacao == 5)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
                }
                else if (pesqParams.Ordenacao == 7)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.NomeSerie).ThenBy(x => x.SemanaEntrega).ToList();
                }
            }
            else
            {
                resLista = resLista.OrderBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            return resLista;
        }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> OrdernarLista(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
    List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> lista)
        {
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> resLista = lista;
            if (pesqParams.Ordenacao == 0)
            {
                resLista = resLista.OrderBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.Ordenacao == 1)
            {
                //Semana Entrega
                resLista = resLista.OrderBy(x => x.SemanaEntrega).ToList();

            }
            else if (pesqParams.Ordenacao == 2)
            {
                resLista = resLista.OrderBy(x => x.NomeCliente).ToList();
            }
            else if (pesqParams.Ordenacao == 3)
            {
                //Cliente (Agrupar)
                resLista = resLista.OrderBy(x => x.NomeCliente).ToList();
                string lastNomeCliente = "";
                int listaCountFixo = resLista.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (resLista[i].NomeCliente != lastNomeCliente)
                    {
                        resLista.Add(new Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin() { NomeCliente = resLista[i].NomeCliente, NomeSerie = "0", NumDoc = 0, Separador = true });
                    }
                    lastNomeCliente = resLista[i].NomeCliente;
                }

                resLista = resLista.OrderBy(x => x.NomeCliente).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (pesqParams.Ordenacao == 4 || pesqParams.Ordenacao == 6)
            {
                //Tipo de obra
                if (pesqParams.Ordenacao == 4)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ToList();

                }
                else if (pesqParams.Ordenacao == 6)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.SemanaEntrega).ThenBy(x => x.NumDoc).ToList();
                }

            }
            else if (pesqParams.Ordenacao == 5 || pesqParams.Ordenacao == 7)
            {
                //Tipo de obra (Agrupar)
                resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ToList();
                string lastNomeTipoEncomenda = "";
                int listaCountFixo = resLista.Count();
                for (int i = 0; i < listaCountFixo; i++)
                {
                    if (resLista[i].NomeTipoEncomenda != lastNomeTipoEncomenda)
                    {
                        resLista.Add(new Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin() { NomeCliente = resLista[i].NomeCliente, NomeTipoEncomenda = resLista[i].NomeTipoEncomenda, NomeSerie = "0", NumDoc = 0, Separador = true });
                    }
                    lastNomeTipoEncomenda = resLista[i].NomeTipoEncomenda;
                }

                if (pesqParams.Ordenacao == 5)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
                }
                else if (pesqParams.Ordenacao == 7)
                {
                    resLista = resLista.OrderBy(x => x.NomeTipoEncomenda).ThenBy(x => x.SemanaEntrega).ThenBy(x => x.NumDoc).ToList();
                }
            }
            else
            {
                resLista = resLista.OrderBy(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            return resLista;
        }


        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> FiltrarData(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> lista)
        {
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> resLista = lista;

            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> aRemover =
               new List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2>();

            foreach (var item in resLista)
            {
                item.DataPedidoString = item.DataPedido.ToString(@"yyyy-MM-dd");
                if (pesqParams.DataEntregaAteBool || pesqParams.DataEntregaDesdeBool)
                {
                    DateTime dt = new DateTime();
                    bool dtParse = DateTime.TryParse(item.DataPedidoString, out dt);
                    if (dtParse)
                    {
                        if (pesqParams.DataEntregaAteBool && !(DateTime.Compare(pesqParams.DataEntregaAteValue, dt) > 0))
                        {
                            aRemover.Add(item);
                        }

                        if (pesqParams.DataEntregaDesdeBool && !(DateTime.Compare(pesqParams.DataEntregaDesdeValue, dt) < 0))
                        {
                            aRemover.Add(item);
                        }
                    }
                    else
                    {
                        aRemover.Add(item);
                    }
                }
            }

            foreach (var item in aRemover)
            {
                resLista.Remove(item);
            }
            return resLista;
        }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> FiltrarData(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
        List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> lista)
        {
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> resLista = lista;

            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> aRemover =
               new List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd>();

            foreach (var item in resLista)
            {
                item.DataPedidoString = item.DataPedido.ToString(@"yyyy-MM-dd");
                item.DataAprovadoString = item.DataAprovado.ToString(@"yyyy-MM-dd");

                if (pesqParams.DataEntregaAteBool || pesqParams.DataEntregaDesdeBool)
                {
                    DateTime dt = new DateTime();
                    bool dtParse = DateTime.TryParse(item.DataAprovadoString, out dt);
                    if (dtParse)
                    {
                        if (pesqParams.DataEntregaAteBool && !(DateTime.Compare(pesqParams.DataEntregaAteValue, dt) > 0))
                        {
                            aRemover.Add(item);
                        }

                        if (pesqParams.DataEntregaDesdeBool && !(DateTime.Compare(pesqParams.DataEntregaDesdeValue, dt) < 0))
                        {
                            aRemover.Add(item);
                        }
                    }
                    else
                    {
                        aRemover.Add(item);
                    }
                }
            }

            foreach (var item in aRemover)
            {
                resLista.Remove(item);
            }
            return resLista;
        }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> FiltrarData(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
    List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> lista)
        {
            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> resLista = lista;

            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> aRemover =
               new List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin>();

            foreach (var item in resLista)
            {
                item.DataPedidoString = item.DataPedido.ToString(@"yyyy-MM-dd");
                item.DataAprovadoString = item.DataAprovado.ToString(@"yyyy-MM-dd");

                if (pesqParams.DataEntregaAteBool || pesqParams.DataEntregaDesdeBool)
                {
                    DateTime dt = new DateTime();
                    bool dtParse = DateTime.TryParse(item.DataAprovadoString, out dt);
                    if (dtParse)
                    {
                        if (pesqParams.DataEntregaAteBool && !(DateTime.Compare(pesqParams.DataEntregaAteValue, dt) > 0))
                        {
                            aRemover.Add(item);
                        }

                        if (pesqParams.DataEntregaDesdeBool && !(DateTime.Compare(pesqParams.DataEntregaDesdeValue, dt) < 0))
                        {
                            aRemover.Add(item);
                        }
                    }
                    else
                    {
                        aRemover.Add(item);
                    }
                }
            }

            foreach (var item in aRemover)
            {
                resLista.Remove(item);
            }
            return resLista;
        }


        public NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaRes PesqParamRes(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
#if DEBUG
            Stopwatch timer = Stopwatch.StartNew();
#endif
            IEnumerable<Models.Encomendas> encs = GetEncomendasFiltered(pesqParams);

            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> resLista = ConvertEncomendasToEncomendasLinha2(encs);
            resLista = OrdernarLista(pesqParams, resLista);

            resLista = FiltrarData(pesqParams, resLista);

            NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaRes res = new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaRes();
            res.Encomendas = resLista;
            res.AgruparCliente = pesqParams.Ordenacao == 3 || pesqParams.Ordenacao == 5;
            res.Ordenacao = pesqParams.Ordenacao;

#if DEBUG
            timer.Stop();
            Console.WriteLine("TIME : " + timer.ElapsedMilliseconds);
#endif
            return res;
        }



        public NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaProd PesqProdParamRes(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
#if DEBUG
            Stopwatch timer = Stopwatch.StartNew();
#endif
            IEnumerable<Models.Encomendas> encs = GetEncomendasFiltered(pesqParams);

            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> resLista = ConvertEncomendasToEncomendasProd(encs);
            resLista = OrdernarLista(pesqParams, resLista);

            resLista = FiltrarData(pesqParams, resLista);

            NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaProd res = new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaProd();
            res.Encomendas = resLista;
            res.AgruparCliente = pesqParams.Ordenacao == 3 || pesqParams.Ordenacao == 5;
            res.Ordenacao = pesqParams.Ordenacao;

#if DEBUG
            timer.Stop();
            Console.WriteLine("TIME : " + timer.ElapsedMilliseconds);
#endif


            return res;
        }


        public NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaAdmin PesqAdminParamRes(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {

            IEnumerable<Models.Encomendas> encs = GetEncomendasFiltered(pesqParams);

            List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> resLista = ConvertEncomendasToEncomendasLinhaAdmin(encs);
            resLista = OrdernarLista(pesqParams, resLista);

            resLista = FiltrarData(pesqParams, resLista);

            NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaAdmin res = new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaAdmin();
            res.Encomendas = resLista;
            res.AgruparCliente = pesqParams.Ordenacao == 3 || pesqParams.Ordenacao == 5;
            res.Ordenacao = pesqParams.Ordenacao;

            return res;
        }


        public IEnumerable<Models.Encomendas> GetEncomendasFiltered(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)

        {
            IEnumerable<Models.Encomendas> encs = EncomendasCache.GetEncomendasListaComClienteETipoEncomenda();// this.DbContext.Encomendas.Include(x => x.Cliente).Include(x => x.TipoEncomenda);

            if (pesqParams.Clientes.Any())
            {
                int[] numClientes = pesqParams.Clientes.Select(x => x.NumCliente).ToArray();
                encs = encs.Where(x => numClientes.Any(z => z == x.Cliente.NumCliente));
            }

            if (pesqParams.Serie != "" && pesqParams.Serie != "--")
            {
                encs = encs.Where(x => x.SerieDoc.NumSerie == pesqParams.Serie);
            }

            if (pesqParams.NumDocDesdeBool && pesqParams.NumDocDesdeValue != 0)
            {
                encs = encs.Where(x => x.NumDoc >= pesqParams.NumDocDesdeValue);
            }

            if (pesqParams.NumDocAteBool && pesqParams.NumDocAteValue != 0)
            {
                encs = encs.Where(x => x.NumDoc <= pesqParams.NumDocAteValue);
            }

            //startDate <= order.OrderDate && order.OrderDate < endDate
            if (pesqParams.DataPedidoDesdeBool)
            {
                encs = encs.Where(x => DateTime.Compare(pesqParams.DataPedidoDesdeValue, x.DataPedido) < 0);
            }

            if (pesqParams.DataPedidoAteBool)
            {
                encs = encs.Where(x => DateTime.Compare(pesqParams.DataPedidoAteValue, x.DataPedido) > 0);
            }

            if (pesqParams.SemanaEntregaDesdeBool || (pesqParams.SemanaEntregaAteBool))
            {
                //recheck max e min
                int max = Math.Max(pesqParams.SemanaEntregaAteValue, pesqParams.SemanaEntregaDesdeValue);
                int min = Math.Min(pesqParams.SemanaEntregaAteValue, pesqParams.SemanaEntregaDesdeValue);

                if (pesqParams.SemanaEntregaDesdeBool)
                {
                    encs = encs.Where(x => min <= x.SemanaEntrega);
                }

                if (pesqParams.SemanaEntregaAteBool)
                {
                    encs = encs.Where(x => max >= x.SemanaEntrega);
                }
            }

            if (pesqParams.EstadosEncomenda.Count() != 0)
            {
                int[] estadosEncomenda = pesqParams.EstadosEncomenda.Select(x => x.NumEstado).ToArray();
                encs = encs.Where(x => estadosEncomenda.Any(z => z == x.Estado));
            }

            if (pesqParams.TipoEncomenda.Count() != 0)
            {
                int[] tipoEncomenda = pesqParams.TipoEncomenda.Select(x => x.NumTipoEncomenda).ToArray();
                encs = encs.Where(x => x.TiposEncomenda.Any(z => tipoEncomenda.Any(y => z.TipoEncomendaId == y)));  //  tipoEncomenda.Any(z => z == x.TipoEncomenda.NumTipoEncomenda));

            }

            if (!(pesqParams.RefObra == null || pesqParams.RefObra.Trim() == string.Empty))
            {
                string valorProcObra = pesqParams.RefObra.ToLowerInvariant();
                encs = encs.Where(x => (x.RefObra.ToLower().IndexOf(valorProcObra) != -1));
            }

            if (pesqParams.MostrarGuiaRemessados == 1)
            {
                //mostra todos guiaRemessados
                //nao necessita de filtragem
            }
            else if (pesqParams.MostrarGuiaRemessados == 2)
            {
                //mostra so nao guiaRemessados
                encs = encs.Where(x => (x.GuiaRemessa == null || x.GuiaRemessa.Trim() == string.Empty));
            }
            else if (pesqParams.MostrarGuiaRemessados == 3)
            {
                //mostra so guiaRemessados
                if (pesqParams.GuiaRemessa == null || pesqParams.GuiaRemessa.Trim() == string.Empty)
                {
                    encs = encs.Where(x => !(x.GuiaRemessa == null || x.GuiaRemessa.Trim() == string.Empty));
                }
                else
                {
                    string valorProc = pesqParams.GuiaRemessa.Trim().ToLowerInvariant();
                    encs = encs.Where(x => (x.GuiaRemessa.ToLower().IndexOf(valorProc) != -1));
                }
            }

            if (pesqParams.DataEntregaAteBool || pesqParams.DataEntregaDesdeBool)
            {
                encs = encs.Where(x => (x.DataEntrega.HasValue));
            }
            return encs;
        }


        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> ConvertEncomendasToEncomendasLinha2(IEnumerable<Models.Encomendas> encs)
        {
            return encs.Select(x => new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2()
            {
                Id = x.IdEncomenda,
                NumDoc = x.NumDoc,
                NomeSerie = x.SerieDoc.NumSerie,
                SerieNumEncomenda = x.SerieDoc.NumSerie + @"/" + x.NumDoc,
                NomeCliente = x.Cliente.NomeCliente,
                RefObra = string.IsNullOrEmpty(x.RefObra) ? "" : x.RefObra,
                NomeTipoEncomenda = x.TiposEncomenda.Select(z => z.TipoEncomendas.NomeTipoEncomenda).Aggregate((i, j) => i + ", " + j), //x.TipoEncomenda.NomeTipoEncomenda,
                DataPedido = x.DataPedido, //x.DataPedido.ToString(@"yyyy/MM/dd"),                
                GuiaRemessa = string.IsNullOrEmpty(x.GuiaRemessa) ? "" : x.GuiaRemessa,
                Estado = x.Estado,
                DataEntregaString = x.DataEntrega.HasValue ? x.DataEntrega.Value.ToString(@"yyyy/MM/dd") : "",
                SemanaEntrega = x.SemanaEntrega,
                NumVidros = x.NumVidros
            }).ToList();
        }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> ConvertEncomendasToEncomendasProd(IEnumerable<Models.Encomendas> encs)
        {
            return encs.Select(x => new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd()
            {
                Id = x.IdEncomenda,
                NumDoc = x.NumDoc,
                NomeSerie = x.SerieDoc.NumSerie,
                SerieNumEncomenda = x.SerieDoc.NumSerie + @"/" + x.NumDoc,
                NomeCliente = x.Cliente.NomeCliente,
                RefObra = string.IsNullOrEmpty(x.RefObra) ? "" : x.RefObra,
                NomeTipoEncomenda = x.TiposEncomenda.Select(z => z.TipoEncomendas.NomeTipoEncomenda).Aggregate((i, j) => i + ", " + j),
                Estado = x.Estado,
                SemanaEntrega = x.SemanaEntrega,
                NumVidros = x.NumVidros,
                DataEntregaString = x.DataEntrega.HasValue ? x.DataEntrega.Value.ToString(@"yyyy/MM/dd") : "",
                DataExpedido = x.DataExpedido, //x.DataPedido.ToString(@"yyyy/MM/dd"),                

            }).ToList();
        }

        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> ConvertEncomendasToEncomendasLinhaAdmin(IEnumerable<Models.Encomendas> encs)
        {
            return encs.Select(x => new NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin()
            {
                Id = x.IdEncomenda,
                NumDoc = x.NumDoc,
                NomeSerie = x.SerieDoc.NumSerie,
                SerieNumEncomenda = x.SerieDoc.NumSerie + @"/" + x.NumDoc,
                NomeCliente = x.Cliente.NomeCliente,
                RefObra = string.IsNullOrEmpty(x.RefObra) ? "" : x.RefObra,
                NomeTipoEncomenda = x.TiposEncomenda.Select(z => z.TipoEncomendas.NomeTipoEncomenda).Aggregate((i, j) => i + ", " + j),
                DataPedido = x.DataPedido,
                GuiaRemessa = string.IsNullOrEmpty(x.GuiaRemessa) ? "" : x.GuiaRemessa,
                Estado = x.Estado,
                SemanaEntrega = x.SemanaEntrega,
                DataEntregaString = x.DataEntrega.HasValue ? x.DataEntrega.Value.ToString(@"yyyy/MM/dd") : "",
                NumVidros = x.NumVidros
            }).ToList();
        }

        public async Task<NVidrosEncomendas.WebServer.ViewModels.GerarRelatorioRes> GerarRelatorio(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParam)
        {

            string tpString = pesqParam.TipoRetorno;

            //var _reportingService = new ReportingService("https://devis.jsreportonline.net", "admin", "IgrejaNova");            
            var _reportingService = new ReportingService("https://encomendasdotnfermeturesdotcom.jsreportonline.net", "admin2", "IgrejaNova");

            var headerStream = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/Encomendas/0Header.html"));
            string header = headerStream.ReadToEnd();
            headerStream.Close();
            headerStream.Dispose();
            StreamReader contentStream;
            if (tpString.Contains("xlsx"))
            {
                contentStream = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/Encomendas/1contentXlsx.html"));
            }
            else
            {
                contentStream = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports/Encomendas/1content.html"));
            }

            string content = contentStream.ReadToEnd();
            contentStream.Close();
            contentStream.Dispose();

            EncomendasBL ebl = new EncomendasBL();
            var dados = ebl.PesqParamRes(pesqParam);

            string dadosString = Newtonsoft.Json.JsonConvert.SerializeObject(dados);
            var reportResult = await _reportingService.RenderAsync(new RenderRequest()
            {
                template = new Template()
                {
                    recipe = tpString,// "phantom-pdf",
                    content = content,
                    engine = "jsrender",

                    phantom = new Phantom()
                    {
                        header = header,
                        footer = "<div style='text-align:right'>{#pageNum}/{#numPages}</div> ",
                        orientation = "landscape",
                        headerHeight = "0px",
                        margin = "0.5 cm"
                    }
                },
                data = dados
            });


            NVidrosEncomendas.WebServer.Controllers.AccountController ac = new Controllers.AccountController();

            Relatorios r = new Relatorios();
            r.Controller = "EncomendasBL";
            r.Method = "GerarRelatorio";
            r.NomeUtilizador = ac.GetMe().Login;
            r.DataGerado = DateTime.Now;
            r.HtmlQuery = Newtonsoft.Json.JsonConvert.SerializeObject(pesqParam);
            r.UniqueId = Guid.NewGuid().ToString();
            AppDbContext context = new AppDbContext();
            context.Relatorios.Add(r);
            context.SaveChanges();


            string fileName = "/Exports/Relatorios/" + r.Id + "." + (reportResult.FileExtension ?? "pdf");
            MemoryStream memory = new MemoryStream();

            FileStream file1 = File.Create(HttpRuntime.AppDomainAppPath + fileName);
            reportResult.Content.CopyTo(file1);
            file1.Close();

            r.NomeFicheiro = fileName;
            r.TipoFicheiro = "PDF";
            context.SaveChanges();

            NVidrosEncomendas.WebServer.ViewModels.GerarRelatorioRes res = new NVidrosEncomendas.WebServer.ViewModels.GerarRelatorioRes();

            res.UniqueId = r.UniqueId;
            EncomendasCache.UpdateCache = true;

            return res;
        }

    }
}