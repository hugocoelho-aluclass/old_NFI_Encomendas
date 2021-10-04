using NVidrosEncomendas.WebServer.BusinessLogic;
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class DashboardController : ApiController
    {
        [HttpGet]
        public ViewModels.Dashboard DashboardHome()
        {
            ViewModels.Dashboard res = new ViewModels.Dashboard();

            EncomendasBL ebl = new EncomendasBL();

            var todasEncomendas = EncomendasCache.GetEncomendasLista();// EncomendasCache.GetEncomendasLista();

            res.TotalEncomendas = todasEncomendas.Count();
            res.TotalExpedidas = 0;
            res.TotalPendentes = todasEncomendas.Where(x => x.Estado == 0).Count();
            res.TotalProducao = todasEncomendas.Where(x => x.Estado == 1).Count();

            List<ViewModels.EncomendaDashboard> encs = new List<ViewModels.EncomendaDashboard>();

            encs = (from c in todasEncomendas
                    where c.Estado == 0 || c.Estado == 1
                    select new
                        ViewModels.EncomendaDashboard
                    {
                        IdEncomenda = c.IdEncomenda,                        
                        NumVidros = c.NumVidros
                    }
                ).OrderBy(x => x.DataPedido).ToList();
       

            res.EncomendasSemEntrega = encs.Where(x => x.SemanaEntrega == 0).ToList();

            Helper hp = new Helper();
            int semanaHoje = hp.GetWeekOfYear(DateTime.Now);

            res.TotalEncSemSemana = (from c in todasEncomendas
                                     where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega == 0
                                     select c.IdEncomenda).Count();

            res.TotalEncAtrasada = (from c in todasEncomendas
                                    where (c.Estado == 0 || c.Estado == 1) && ((semanaHoje - c.SemanaEntrega) > 0 && (semanaHoje - c.SemanaEntrega) < 40) && c.SemanaEntrega != 0// &&  c.SemanaEntrega < semanaHoje
                                    select c.IdEncomenda).Count();

            res.TotalEncEstaSemana = (from c in todasEncomendas
                                      where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega == semanaHoje
                                      select c.IdEncomenda).Count();

            res.TotalEncProxSemana1 = (from c in todasEncomendas
                                       where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == 1) || (((52 - semanaHoje) + c.SemanaEntrega) == 1))
                                       select c.IdEncomenda).Count();

            res.TotalEncProxSemana2 = (from c in todasEncomendas
                                       where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == 2) || (((52 - semanaHoje) + c.SemanaEntrega) == 2))
                                       select c.IdEncomenda).Count();


            res.TotalEncProxSemana3 = (from c in todasEncomendas
                                       where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == 3) || (((52 - semanaHoje) + c.SemanaEntrega) == 3))
                                       select c.IdEncomenda).Count();


            res.TotalEncProxSemana4mais = (from c in todasEncomendas
                                           where (c.Estado == 0 || c.Estado == 1) && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) > 3) || (hp.GetWeekOfYear(c.DataPedido) > c.SemanaEntrega && ((52 - semanaHoje) + c.SemanaEntrega) > 3))
                                           select c.IdEncomenda).Count();

            res.TotalEntreguesNaoGuiaRemessadas = (from c in todasEncomendas
                                              where (c.Estado == 2) && c.GuiaRemessa == ""
                                              select c.IdEncomenda).Count();

            return res;
        }

        [HttpGet]
        //[Route("dashboardHomeDate/date:string")]
        public ViewModels.Dashboard DashboardHomeDate(string id)
        {
            ViewModels.Dashboard res = new ViewModels.Dashboard();
            string[] dataSplited = id.Split('-');
            DateTime dtMin = new DateTime(Int32.Parse(dataSplited[0]), Int32.Parse(dataSplited[1]), Int32.Parse(dataSplited[2]));

            EncomendasBL ebl = new EncomendasBL();
            var encomendasLista = EncomendasCache.GetEncomendasLista();// EncomendasCache.GetEncomendasLista();
            //var encomendasLista = EncomendasCache.GetEncomendasLista();

            res.TotalEncomendas = encomendasLista.Where(x => x.DataPedido >= dtMin).Count();
            res.TotalExpedidas = 0;
            res.TotalPendentes = encomendasLista.Where(x => x.Estado == 0 && x.DataPedido >= dtMin).Count();
            res.TotalProducao = encomendasLista.Where(x => x.Estado == 1 && x.DataPedido >= dtMin).Count();

            List<ViewModels.EncomendaDashboard> encs = new List<ViewModels.EncomendaDashboard>();

            encs = (from c in encomendasLista
                    where c.Estado == 0 || c.Estado == 1
                    select new
                        ViewModels.EncomendaDashboard
                    {
                        IdEncomenda = c.IdEncomenda,
                        NumTipoEncomenda = c.TiposEncomenda.FirstOrDefault().TipoEncomendaId,
                        NumVidros = c.NumVidros,
                        DataPedido = c.DataPedido
                    }
                   ).OrderBy(x => x.DataPedido).ToList();


            //res.EncomendasDashboard = encs;
            //res.EncomendasSemEntrega = encs.Where(x => x.SemanaEntrega == 0 && x.DataPedido >= dtMin).ToList();
            res.TotalCaixilhos = encs.Where(x => x.NumTipoEncomenda == 1 && x.DataPedido >= dtMin).Sum(x => x.NumVidros);
            res.TotalPortoesAp = encs.Where(x => x.NumTipoEncomenda == 2 && x.DataPedido >= dtMin).Sum(x => x.NumVidros);
            res.TotalPortoesSold = encs.Where(x => x.NumTipoEncomenda == 3 && x.DataPedido >= dtMin).Sum(x => x.NumVidros);
            res.TotalEstores = encs.Where(x => x.NumTipoEncomenda == 4 && x.DataPedido >= dtMin).Sum(x => x.NumVidros);

            Helper hp = new Helper();
            int semanaHoje = hp.GetWeekOfYear(DateTime.Now);

            res.TotalEncSemSemana = res.TotalEncEstaSemana = (from c in encomendasLista
                                                              where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega == 0 && c.DataPedido >= dtMin
                                                              select c.IdEncomenda).Count();

            res.TotalEncAtrasada = (from c in encomendasLista
                                    where (c.Estado == 0 || c.Estado == 1) && ((semanaHoje - c.SemanaEntrega) > 0 && (semanaHoje - c.SemanaEntrega) < 40) && c.SemanaEntrega != 0 && c.DataPedido >= dtMin
                                    select c.IdEncomenda).Count();

            res.TotalEncEstaSemana = (from c in encomendasLista
                                      where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega == semanaHoje
                                      && c.DataPedido >= dtMin
                                      select c.IdEncomenda).Count();

            res.TotalEncProxSemana1 = (from c in encomendasLista
                                       where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == 1) || (((52 - semanaHoje) + c.SemanaEntrega) == 1))
                                       && c.DataPedido >= dtMin
                                       select c.IdEncomenda).Count();

            res.TotalEncProxSemana2 = (from c in encomendasLista
                                       where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == 2) || (((52 - semanaHoje) + c.SemanaEntrega) == 2))
                                       && c.DataPedido >= dtMin
                                       select c.IdEncomenda).Count();


            res.TotalEncProxSemana3 = (from c in encomendasLista
                                       where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == 3) || (((52 - semanaHoje) + c.SemanaEntrega) == 3))
                                       && c.DataPedido >= dtMin
                                       select c.IdEncomenda).Count();


            res.TotalEncProxSemana4mais = (from c in encomendasLista
                                           where (c.Estado == 0 || c.Estado == 1) && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) > 3) || (hp.GetWeekOfYear(c.DataPedido) > c.SemanaEntrega && ((52 - semanaHoje) + c.SemanaEntrega) > 3))
                                           && c.DataPedido >= dtMin
                                           select c.IdEncomenda).Count();

 
            res.TotalEntreguesNaoGuiaRemessadas = (from c in encomendasLista
                                              where (c.Estado == 2) && c.GuiaRemessa == ""
                                              && c.DataPedido >= dtMin
                                              select c.IdEncomenda).Count();

            return res;
        }


        [HttpPost]
        public ViewModels.DashboardEncomendas DashboardEncomendas(ViewModels.DashboardReq req)
        {
            int id = req.tipo;
            DateTime dataDesde = req.GetData();

            //EncomendasBL ebl = new EncomendasBL();
            ViewModels.DashboardEncomendas res = new ViewModels.DashboardEncomendas();
            int semanaHoje = (new Helper()).GetWeekOfYear(DateTime.Now);

            if (id == -1)
            {
                res.Encomendas = (from c in EncomendasCache.GetEncomendasLista()
                                  where (c.Estado == 0 || c.Estado == 1) && c.SemanaEntrega == 0 && c.DataPedido >= dataDesde
                                  select new ViewModels.EncomendaDashboard(c)
                                  
                 ).OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (id == -2) //atrasadas
            {
                res.Encomendas = (from c in EncomendasCache.GetEncomendasLista()
                                  where (c.Estado == 0 || c.Estado == 1) && ((semanaHoje - c.SemanaEntrega) > 0 && (semanaHoje - c.SemanaEntrega) < 40) && c.SemanaEntrega != 0
                                  && c.DataPedido >= dataDesde
                                  select new ViewModels.EncomendaDashboard(c)
                ).OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (id == -3) //em producao
            {
                res.Encomendas = (from c in EncomendasCache.GetEncomendasLista()
                                  where (c.Estado == 1) && c.DataPedido >= dataDesde
                                  select new ViewModels.EncomendaDashboard(c)
                ).OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (id == -4) //4 semanas +
            {
                Helper hp = new Helper();
                res.Encomendas = (from c in EncomendasCache.GetEncomendasLista()
                                  where (c.Estado == 0 || c.Estado == 1) && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) > 3) || (hp.GetWeekOfYear(c.DataPedido) > c.SemanaEntrega && ((52 - semanaHoje) + c.SemanaEntrega) > 3))
                                   && c.DataPedido >= dataDesde
                                  select new ViewModels.EncomendaDashboard(c)
                ).OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else if (id == -5) //sem material encomendado
            {
                res.Encomendas = (from c in EncomendasCache.GetEncomendasLista()
                                  where (c.Estado == 0 || c.Estado == 1) 
                                  && c.DataPedido >= dataDesde
                                  select new ViewModels.EncomendaDashboard(c)
                    ).OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            else
            {
                res.Encomendas = (from c in EncomendasCache.GetEncomendasLista()
                                  where (c.Estado == 0 || c.Estado == 1) && (c.SemanaEntrega != 0 && ((c.SemanaEntrega >= semanaHoje && (c.SemanaEntrega - semanaHoje) == id) || (((52 - semanaHoje) + c.SemanaEntrega) == id)))
                                  && c.DataPedido >= dataDesde
                                  select new ViewModels.EncomendaDashboard(c)
                ).OrderByDescending(x => x.NomeSerie).ThenBy(x => x.NumDoc).ToList();
            }
            return res;
        }

        [HttpPost]
        public string MarcarExpedida(int encId, string dataExp, string guiaRemessa)
        {
            EncomendasBL encbl = new EncomendasBL();
            Models.Encomendas encs = encbl.LerEncomenda(encId).Key;
            encs.Estado = 2;           
            encs.GuiaRemessa = guiaRemessa;
            encbl.AtualizaEncomenda(encs);
            return "sucesso";
        }

        [HttpPost]
        public string AtualizaEstado(int encId, int estado)
        {
            EncomendasBL encbl = new EncomendasBL();
            Models.Encomendas encs = encbl.LerEncomenda(encId).Key;
            encs.Estado = estado;
            encbl.AtualizaEncomenda(encs);
            return "sucesso";
        }

        [HttpPost]
        public string AlterarPassword(string login, string pwAntiga, string pwNova)
        {
            AdminUtilizadoresBL abl = new AdminUtilizadoresBL();
            string res = abl.AtualizarPassword(login, pwAntiga, pwNova);

            return res;

        }
    }
}
