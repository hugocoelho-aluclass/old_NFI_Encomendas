using jsreport.Client;
using jsreport.Client.Entities;
using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NfiEncomendas.WebServer.BusinessLogic
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
            return DbContext.Encomendas.Include("SerieDoc").Include("Cliente").Include("TipoEncomenda").Where(x => x.Anulada == false).OrderBy(x => x.SerieDoc.NumSerie).ThenBy(x => x.NumDoc);
        }

        public bool ExisteEncomendaNum(int num, string ano)
        {

            var sql = "SELECT COUNT(*) FROM dbo.Encomendas WHERE (NumDoc = '" + num + "') AND (SerieDoc_NumSerie = '" + ano + "')";
            var res = DbContext.Database.SqlQuery<int>(sql).First();

            /*var sql = "SELECT CAST(" +
                        "CASE WHEN EXISTS(SELECT * FROM dbo.Encomendas WHERE NumDoc = " + num + ") THEN 1" +
                        "ELSE 0" +
                        "END" +
                         "AS BIT)";
            var res = DbContext.Database.SqlQuery<bool>(sql).First();*/

            if (res == 0)
            {
                return false;
            }

            return true;

            /*return res;*/
        }

        public KeyValuePair<Models.Encomendas, bool> LerEncomenda(int id)
        {
            var res = DbContext.Encomendas.Include("EncomendasCompras").Where(x => x.IdEncomenda == id).FirstOrDefault();
            bool nova = res == null;
            if (res == null)
            {
                res = new Encomendas();

                SeriesBL sbl = new SeriesBL(DbContext);
                res.SerieDoc = sbl.SerieDefeito();
                res.NumDoc = res.SerieDoc.UltimoDoc + 1;
            }

            return new KeyValuePair<Models.Encomendas, bool>(res, nova);
        }

        public KeyValuePair<Models.Encomendas, bool> LerEncomenda(string serie, int numDoc)
        {
            var res = DbContext.Encomendas.Include("EncomendasCompras").Where(x => x.SerieDoc.NumSerie == serie && x.NumDoc == numDoc).FirstOrDefault();
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
                TipoEncomendas tcCl = enc.TipoEncomenda;
                Encomendas _sr = DbContext.Encomendas.Include("SerieDoc").Include("Cliente").Include("TipoEncomenda").Include("EncomendasCompras").Where(x => x.IdEncomenda == enc.IdEncomenda && x.NumDoc == enc.NumDoc && x.SerieDoc.NumSerie == enc.SerieDoc.NumSerie).FirstOrDefault();
                bool novo = false;

                if (_sr == null)
                {
                    novo = true;
                    _sr = new Encomendas();
                    _sr.NumDoc = enc.SerieDoc.UltimoDoc + 1;
                    //_sr.NumEncomenda = sr.NumEncomenda;
                }
                else
                {
                    _sr.NumDoc = enc.NumDoc;
                }

                _sr.SerieDoc = enc.SerieDoc;

                _sr.Cliente = encCl;// enc.Cliente;
                _sr.TipoEncomenda = tcCl;//enc.TipoEncomenda;
                _sr.NomeArtigo = enc.NomeArtigo;
                _sr.Cor = enc.Cor;
                _sr.Painel = enc.Painel;
                _sr.Producao = enc.Producao;
                _sr.SemanaEntrega = enc.SemanaEntrega;
                _sr.DataPedido = enc.DataPedido;
                _sr.SemanaEntrega = enc.SemanaEntrega;
                _sr.DataExpedido = DateTime.Now.Date;
                _sr.DataExpedidoString = enc.DataExpedidoString;
                _sr.Fatura = enc.Fatura;
                _sr.Notas = enc.Notas;
                _sr.Anulada = enc.Anulada;
                _sr.NumVaos = enc.NumVaos;
                _sr.Estado = enc.Estado;
                _sr.NumSerieEncomenda = enc.NumSerieEncomenda;

                bool novaCompra = false;
                bool comprasOK = true;
                foreach (var item in enc.EncomendasCompras)
                {
                    var ec = _sr.EncomendasCompras.Where(x => item.IdCompraEncomendas != 0 && item.IdCompraEncomendas == x.IdCompraEncomendas).FirstOrDefault();
                    if (ec == null)
                    {
                        ec = new EncomendasCompras();
                        novaCompra = true;
                    }
                    ec.Material = item.Material;
                    ec.NotasFornecedor = item.NotasFornecedor;
                    ec.LinhaCompra = item.LinhaCompra;
                    ec.DataPedido = item.DataPedido;
                    ec.DataEntrega = item.DataEntrega;
                    if (novaCompra)
                    {
                        novaCompra = false;
                    }

                    if (ec.DataPedido.HasValue && !ec.DataEntrega.HasValue)
                    {
                        comprasOK = false;
                    }
                }

                _sr.ComprasOK = (_sr.Estado <= 1) ? comprasOK : true;
                //SeriesBL serieBl = new SeriesBL(DbContext);

                //serieBl.AtualizaUltimoDocSerie(_sr.SerieDoc.NumSerie);

                if (novo)
                {
                    _sr.NumDoc = enc.SerieDoc.UltimoDoc + 1;
                    _sr.SerieDoc.UltimoDoc = _sr.NumDoc;
                    // Valida se existe já alguma encomenda com o mesmo número NumDoc
                    bool valida = false;
                    // Variavel para evitar que entre num loop infinito
                    var x = 0;
                    while (valida == false || x > 10)
                    {
                        //Verifica se já existe uma encomenda como mesmo NumDoc
                        if (ExisteEncomendaNum(_sr.NumDoc, _sr.SerieDoc.NumSerie))
                        {
                            // se existir uma encomenda com o mesmo NumDoc, encrementa o número do NumDoc e o numero da ultima encomenda da serie para a próximo ciclo do loop
                            _sr.NumDoc++;
                            _sr.SerieDoc.UltimoDoc = _sr.NumDoc;
                        }
                        else
                        {
                            // se não existir uma encomenda com o mesmo NumDoc, adiciona a encomenda e atualiza na BD
                            DbContext.Encomendas.Add(_sr);
                            valida = true;
                        }
                        x++;
                    }


                }
                EncomendasCache.UpdateCache = true;
                DbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }


        public void AtualizaComprasEncomendas()
        {
            List<Encomendas> encomendas = DbContext.Encomendas.Include("EncomendasCompras").ToList();

            for (int i = 0; i < encomendas.Count(); i++)
            {
                int comprasNaoOK = encomendas[i].EncomendasCompras.Count(x => x.DataPedido.HasValue && !x.DataEntrega.HasValue);
                encomendas[i].ComprasOK = (encomendas[i].Estado <= 1) ? (comprasNaoOK == 0) : true;
            }
            EncomendasCache.UpdateCache = true;

            DbContext.SaveChanges();
        }

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> OrdernarLista(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> lista)
        {
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> resLista = lista;
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

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> OrdernarLista(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
       List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> lista)
        {
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> resLista = lista;
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

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> OrdernarLista(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
    List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> lista)
        {
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> resLista = lista;
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


        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> FiltrarData(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> lista)
        {
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> resLista = lista;

            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> aRemover =
               new List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2>();

            foreach (var item in resLista)
            {
                item.DataPedidoString = item.DataPedido.ToString(@"yyyy-MM-dd");
                if (pesqParams.DataEntregaAteBool || pesqParams.DataEntregaDesdeBool)
                {
                    DateTime dt = new DateTime();
                    bool dtParse = DateTime.TryParse(item.DataExpedido, out dt);
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

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> FiltrarData(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
        List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> lista)
        {
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> resLista = lista;

            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> aRemover =
               new List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd>();

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

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> FiltrarData(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams,
    List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> lista)
        {
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> resLista = lista;

            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> aRemover =
               new List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin>();

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


        public NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaRes PesqParamRes(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
#if DEBUG
            Stopwatch timer = Stopwatch.StartNew();
#endif
            IEnumerable<Models.Encomendas> encs = GetEncomendasFiltered(pesqParams);

            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> resLista = ConvertEncomendasToEncomendasLinha2(encs);
            resLista = OrdernarLista(pesqParams, resLista);

            resLista = FiltrarData(pesqParams, resLista);

            NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaRes res = new NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaRes();
            res.Encomendas = resLista;
            res.AgruparCliente = pesqParams.Ordenacao == 3 || pesqParams.Ordenacao == 5;
            res.Ordenacao = pesqParams.Ordenacao;

#if DEBUG
            timer.Stop();
            Console.WriteLine("TIME : " + timer.ElapsedMilliseconds);
#endif
            return res;
        }



        public NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaProd PesqProdParamRes(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {
#if DEBUG
            Stopwatch timer = Stopwatch.StartNew();
#endif
            IEnumerable<Models.Encomendas> encs = GetEncomendasFiltered(pesqParams);

            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> resLista = ConvertEncomendasToEncomendasProd(encs);
            resLista = OrdernarLista(pesqParams, resLista);

            resLista = FiltrarData(pesqParams, resLista);

            NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaProd res = new NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaProd();
            res.Encomendas = resLista;
            res.AgruparCliente = pesqParams.Ordenacao == 3 || pesqParams.Ordenacao == 5;
            res.Ordenacao = pesqParams.Ordenacao;

#if DEBUG
            timer.Stop();
            Console.WriteLine("TIME : " + timer.ElapsedMilliseconds);
#endif


            return res;
        }


        public NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaAdmin PesqAdminParamRes(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)
        {

            IEnumerable<Models.Encomendas> encs = GetEncomendasFiltered(pesqParams);

            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> resLista = ConvertEncomendasToEncomendasLinhaAdmin(encs);
            resLista = OrdernarLista(pesqParams, resLista);

            resLista = FiltrarData(pesqParams, resLista);

            NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaAdmin res = new NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaAdmin();
            res.Encomendas = resLista;
            res.AgruparCliente = pesqParams.Ordenacao == 3 || pesqParams.Ordenacao == 5;
            res.Ordenacao = pesqParams.Ordenacao;

            return res;
        }


        public IEnumerable<Models.Encomendas> GetEncomendasFiltered(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParams)

        {
            IEnumerable<Models.Encomendas> encs = EncomendasCache.GetEncomendasListaComClienteETipoEncomenda();// this.DbContext.Encomendas.Include("Cliente").Include("TipoEncomenda");

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
                encs = encs.Where(x => tipoEncomenda.Any(z => z == x.TipoEncomenda.NumTipoEncomenda));

            }

            if (!(pesqParams.NomeObra == null || pesqParams.NomeObra.Trim() == string.Empty))
            {
                string valorProcObra = pesqParams.NomeObra.ToLowerInvariant();
                encs = encs.Where(x => (x.NomeArtigo.ToLower().IndexOf(valorProcObra) != -1));
            }

            if (pesqParams.MostrarFaturados == 1)
            {
                //mostra todos faturados
                //nao necessita de filtragem
            }
            else if (pesqParams.MostrarFaturados == 2)
            {
                //mostra so nao faturados
                encs = encs.Where(x => (x.Fatura == null || x.Fatura.Trim() == string.Empty));
            }
            else if (pesqParams.MostrarFaturados == 3)
            {
                //mostra so faturados
                if (pesqParams.Fatura == null || pesqParams.Fatura.Trim() == string.Empty)
                {
                    encs = encs.Where(x => !(x.Fatura == null || x.Fatura.Trim() == string.Empty));
                }
                else
                {
                    string valorProc = pesqParams.Fatura.Trim().ToLowerInvariant();
                    encs = encs.Where(x => (x.Fatura.ToLower().IndexOf(valorProc) != -1));
                }
            }

            if (pesqParams.DataEntregaAteBool || pesqParams.DataEntregaDesdeBool)
            {
                encs = encs.Where(x => !String.IsNullOrEmpty(x.DataExpedidoString));
            }
            return encs;

        }


        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2> ConvertEncomendasToEncomendasLinha2(IEnumerable<Models.Encomendas> encs)
        {
            return encs.Select(x => new NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinha2()
            {
                Id = x.IdEncomenda,
                NumDoc = x.NumDoc,
                NomeSerie = x.SerieDoc.NumSerie,
                SerieNumEncomenda = x.SerieDoc.NumSerie + @"/" + x.NumDoc,
                NomeCliente = x.Cliente.NomeCliente,
                NomeArtigo = string.IsNullOrEmpty(x.NomeArtigo) ? "" : x.NomeArtigo,
                NomeTipoEncomenda = x.TipoEncomenda.NomeTipoEncomenda,
                DataPedido = x.DataPedido, //x.DataPedido.ToString(@"yyyy/MM/dd"),
                DataExpedido = string.IsNullOrEmpty(x.DataExpedidoString) ? "" : x.DataExpedidoString,
                Fatura = string.IsNullOrEmpty(x.Fatura) ? "" : x.Fatura,
                Estado = x.Estado,
                SemanaEntrega = x.SemanaEntrega,
                NumVaos = x.NumVaos
            }).ToList();
        }

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd> ConvertEncomendasToEncomendasProd(IEnumerable<Models.Encomendas> encs)
        {
            return encs.Select(x => new NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaProd()
            {
                Id = x.IdEncomenda,
                NumDoc = x.NumDoc,
                NomeSerie = x.SerieDoc.NumSerie,
                SerieNumEncomenda = x.SerieDoc.NumSerie + @"/" + x.NumDoc,
                NomeCliente = x.Cliente.NomeCliente,
                NomeArtigo = string.IsNullOrEmpty(x.NomeArtigo) ? "" : x.NomeArtigo,
                NomeTipoEncomenda = x.TipoEncomenda.NomeTipoEncomenda,
                Cor = x.Cor,
                DataAprovado = x.DataAprovacao.HasValue ? x.DataAprovacao.Value : new DateTime(2019, 01, 01),
                Estado = x.Estado,
                SemanaEntrega = x.SemanaEntrega,
                NumVaos = x.NumVaos,
                DataExpedido = string.IsNullOrEmpty(x.DataExpedidoString) ? "" : x.DataExpedidoString,
            }).ToList();
        }

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin> ConvertEncomendasToEncomendasLinhaAdmin(IEnumerable<Models.Encomendas> encs)
        {
            return encs.Select(x => new NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.EncomendasLinhaAdmin()
            {
                Id = x.IdEncomenda,
                NumDoc = x.NumDoc,
                NomeSerie = x.SerieDoc.NumSerie,
                SerieNumEncomenda = x.SerieDoc.NumSerie + @"/" + x.NumDoc,
                NomeCliente = x.Cliente.NomeCliente,
                NomeArtigo = string.IsNullOrEmpty(x.NomeArtigo) ? "" : x.NomeArtigo,
                NomeTipoEncomenda = x.TipoEncomenda.NomeTipoEncomenda,
                DataPedido = x.DataPedido, //x.DataPedido.ToString(@"yyyy/MM/dd"),
                DataExpedido = string.IsNullOrEmpty(x.DataExpedidoString) ? "" : x.DataExpedidoString,
                Fatura = string.IsNullOrEmpty(x.Fatura) ? "" : x.Fatura,
                Estado = x.Estado,
                SemanaEntrega = x.SemanaEntrega,
                NumVaos = x.NumVaos,
                Cor = x.Cor,
                DataAprovado = (x.DataAprovacao != null && x.DataAprovacao.HasValue) ? x.DataAprovacao.Value : new DateTime(2019, 01, 01)
            }).ToList();
        }

        public async Task<NfiEncomendas.WebServer.ViewModels.GerarRelatorioRes> GerarRelatorio(NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.PagEncomendaPesquisaParam pesqParam)
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


            NfiEncomendas.WebServer.Controllers.AccountController ac = new Controllers.AccountController();

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


            string fileName = "/Exports/Relatorios/" + r.Id + "." + reportResult.FileExtension;
            MemoryStream memory = new MemoryStream();

            FileStream file1 = File.Create(HttpRuntime.AppDomainAppPath + fileName);
            reportResult.Content.CopyTo(file1);
            file1.Close();

            r.NomeFicheiro = fileName;
            r.TipoFicheiro = "PDF";
            context.SaveChanges();

            NfiEncomendas.WebServer.ViewModels.GerarRelatorioRes res = new NfiEncomendas.WebServer.ViewModels.GerarRelatorioRes();

            res.UniqueId = r.UniqueId;
            EncomendasCache.UpdateCache = true;

            return res;
        }

        public NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioEncTotais TotalEncomendas(int semana, string serie)
        {
            var sql = "select * from tabelasemanaTotais(" + semana + ", " + serie + ")";
            var total = DbContext.Database.SqlQuery<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioEncTotais>(sql).First();
            return total;
        }

        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioEncTipoEncomenda> TotaisTipoEncomenda(int semana, string serie)
        {
  
            var sql = "select * from tabelasemana(" + semana + ", " + serie + ")";

            var total = DbContext.Database.SqlQuery<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioEncTipoEncomenda>(sql).ToList();
            return total;
        }

        public int TotalEncPrix(int semana)
        {
            var sql = "SELECT COUNT(*) FROM dbo.Encomendas WHERE (Anulada =0) AND (Cliente_IdCliente = '7') AND (semanaEntrega = '" + semana + "')";
            var total = DbContext.Database.SqlQuery<int>(sql).First();
            return total;
        }

        public int TotalEncWis(int semana)
        {
            var sql = "SELECT COUNT(*) FROM dbo.Encomendas WHERE (Anulada =0) AND (Cliente_IdCliente = '383') AND (semanaEntrega = '" + semana + "')";
            var total = DbContext.Database.SqlQuery<int>(sql).First();
            return total;
        }

        public int TotalEncRestantesClientes(int semana)
        {
            var sql = "SELECT COUNT(*) FROM dbo.Encomendas WHERE (Anulada =0) AND (Cliente_IdCliente <> '7') AND (Cliente_IdCliente <> '383') AND (semanaEntrega = '" + semana + "')";
            var total = DbContext.Database.SqlQuery<int>(sql).First();
            return total;
        }

     


    }
}