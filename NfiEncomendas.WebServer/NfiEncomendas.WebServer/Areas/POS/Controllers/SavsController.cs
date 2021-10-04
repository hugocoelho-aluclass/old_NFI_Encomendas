using AutoMapper;
using NfiEncomendas.WebServer.Areas.POS.ViewModels;
using NfiEncomendas.WebServer.Areas.POS.ViewModels.Savs;
using NfiEncomendas.WebServer.BusinessLogic;
using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class SavsController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Savs.Savs> TabelaSavs()
        {
            var res = from c in (new SavsBL()).SavsLista()
                      select new ViewModels.Savs.Savs() { };

            return res.OrderByDescending(x => x.Id).ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        public string AtualizaComprasSavs()
        {
            SavsBL savs = new SavsBL();

            return "savs atualizadas!";

        }

        [HttpGet]
        public ViewModels.Savs.PagEditSav EditSav(int id = 0)
        {
            ViewModels.Savs.PagEditSav res = new ViewModels.Savs.PagEditSav();

            SavsBL savBl = new SavsBL(SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            //EstadoRecolhaBl savBl = new SavsBL(SessionObject.GetMySessionObject(System.Web.HttpContext.Current));

            KeyValuePair<Models.Savs, bool> BlRes = savBl.LerSav(id, SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            NfiEncomendas.WebServer.Models.Savs enc = BlRes.Key;

            res.Sav = Mapper.Map<Models.Savs, SavGet>(enc);

            res.Sav.NovaSav = BlRes.Value;

            res.Clientes = Mapper.Map<IEnumerable<Models.Clientes>, IEnumerable<IdNome>>(
             new ClientesBL(savBl.DbContext).ClientesListaOrdemNome());

            res.EstadoSav = Mapper.Map<IEnumerable<Models.EstadoSav>, IEnumerable<IdNome>>((enc.Estado != null && enc.Estado.MarcaEncerrado == true) ?
                new EstadoSavBL(savBl.DbContext).EstadoSavLista() : new EstadoSavBL(savBl.DbContext).EstadoSavListaNaoEncerra());

            res.TipoAvaria = Mapper.Map<IEnumerable<Models.TipoAvarias>, IEnumerable<IdNomeExtra>>(
            new TipoAvariasBL(savBl.DbContext).TipoAvariasLista());

            res.ProdutoSav = Mapper.Map<IEnumerable<Models.ProdutoSav>, IEnumerable<IdNome>>(
            new ProdutoSavsBL(savBl.DbContext).ProdutoSavsLista());

            res.Departamentos = Mapper.Map<IEnumerable<Models.DepartamentoSav>, IEnumerable<IdNome>>(
                new DepartamentoSavsBL(savBl.DbContext).DepartamentoSavsLista());


            res.Setores = Mapper.Map<IEnumerable<Models.Setor>, IEnumerable<IdNome>>(
                new SetorBL(savBl.DbContext).SetorLista());


            res.Series = (from s in (new SeriesBL(savBl.DbContext)).SeriesLista()
                          select new ViewModels.Savs.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }).ToList();

            res.EstadosRecolha = Mapper.Map<IEnumerable<Models.EstadoRecolha>, IEnumerable<IdNomeText>>(
                 new EstadoRecolhaBL(savBl.DbContext).EstadoRecolhaLista());

            return res;
        }


        [HttpGet]
        public ViewModels.Savs.PagEditSav EditSav(string serie, int numDoc)
        {
            ViewModels.Savs.PagEditSav res = new ViewModels.Savs.PagEditSav();

            SavsBL savBl = new SavsBL(SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            KeyValuePair<Models.Savs, bool> BlRes = savBl.LerSav(serie, numDoc, SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            NfiEncomendas.WebServer.Models.Savs enc = BlRes.Key;

            res.Sav = Mapper.Map<Models.Savs, SavGet>(enc);
            res.Sav.NovaSav = BlRes.Value;
            res.Clientes = Mapper.Map<IEnumerable<Models.Clientes>, IEnumerable<IdNome>>(
             new ClientesBL(savBl.DbContext).ClientesListaOrdemNome());

            res.EstadoSav = Mapper.Map<IEnumerable<Models.EstadoSav>, IEnumerable<IdNome>>((enc.Estado != null && enc.Estado.MarcaEncerrado == true) ?
                new EstadoSavBL(savBl.DbContext).EstadoSavLista() : new EstadoSavBL(savBl.DbContext).EstadoSavListaNaoEncerra());

            res.TipoAvaria = Mapper.Map<IEnumerable<Models.TipoAvarias>, IEnumerable<IdNomeExtra>>(
            new TipoAvariasBL(savBl.DbContext).TipoAvariasLista());

            res.ProdutoSav = Mapper.Map<IEnumerable<Models.ProdutoSav>, IEnumerable<IdNome>>(
            new ProdutoSavsBL(savBl.DbContext).ProdutoSavsLista());

            res.Departamentos = Mapper.Map<IEnumerable<Models.DepartamentoSav>, IEnumerable<IdNome>>(
                new DepartamentoSavsBL(savBl.DbContext).DepartamentoSavsLista());

            res.Setores = Mapper.Map<IEnumerable<Models.Setor>, IEnumerable<IdNome>>(
                new SetorBL(savBl.DbContext).SetorLista());

            res.Series = (from s in (new SeriesBL(savBl.DbContext)).SeriesLista()
                          select new ViewModels.Savs.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }).ToList();
            res.EstadosRecolha = Mapper.Map<IEnumerable<Models.EstadoRecolha>, IEnumerable<IdNomeText>>(
                    new EstadoRecolhaBL(savBl.DbContext).EstadoRecolhaLista());
            return res;
        }


        [HttpPost]
        public string AtualizaSav(ViewModels.Savs.Savs sav)
        {
            SavsBL savBl = new SavsBL();
            Models.Savs c = sav.ToModel(savBl.DbContext);
            savBl.AtualizaSav(c, SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            return "sucesso";
        }

        [HttpPost]
        public string MarcarEncerrada(ViewModels.Savs.Savs sav)
        {
            SavsBL savBl = new SavsBL();
            EstadoSavBL estSavBl = new EstadoSavBL();
            sav.MarcarResolvida = true;
            sav.EstadoSavNum = estSavBl.EstadoSavLista().First(x => x.MarcaEncerrado).IdEstadoSav;
            Models.Savs c = sav.ToModel(savBl.DbContext);
            savBl.AtualizaSav(c, SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            return "sucesso";
        }
        [HttpGet]
        public ViewModels.Savs.PagSavPesquisa PesquisaSav()
        {
            SeriesBL seriesBl = new SeriesBL();
            var mmyObj = SessionObject.GetMySessionObject(System.Web.HttpContext.Current).OperadorObject;
            ViewModels.Savs.PagSavPesquisa res = new ViewModels.Savs.PagSavPesquisa();


            res.Clientes = Mapper.Map<IEnumerable<Models.Clientes>, IEnumerable<IdNome>>(
            new ClientesBL(seriesBl.DbContext).ClientesListaOrdemNome());

            res.EstadoSav = Mapper.Map<IEnumerable<Models.EstadoSav>, IEnumerable<IdNomePreSelect>>(
               new EstadoSavBL(seriesBl.DbContext).EstadoSavLista());

            res.TipoAvaria = Mapper.Map<IEnumerable<Models.TipoAvarias>, IEnumerable<IdNomeExtra>>(
            new TipoAvariasBL(seriesBl.DbContext).TipoAvariasLista());

            res.ProdutoSav = Mapper.Map<IEnumerable<Models.ProdutoSav>, IEnumerable<IdNome>>(
            new ProdutoSavsBL(seriesBl.DbContext).ProdutoSavsLista());

            res.Departamentos = Mapper.Map<IEnumerable<Models.DepartamentoSav>, IEnumerable<IdNomePreSelect>>(
                       new DepartamentoSavsBL(seriesBl.DbContext).DepartamentoSavsLista());

            res.EstadoRecolha = Mapper.Map<IEnumerable<Models.EstadoRecolha>, IEnumerable<IdNome>>(
                    new EstadoRecolhaBL(seriesBl.DbContext).EstadoRecolhaLista());

            res.Setores = Mapper.Map<IEnumerable<Models.Setor>, IEnumerable<IdNome>>(
                new SetorBL(seriesBl.DbContext).SetorLista());

            res.Series = (from s in (new SeriesBL(seriesBl.DbContext)).SeriesLista()
                          select new ViewModels.Savs.Serie
                          {
                              NumSerie = s.NumSerie,
                              UltDoc = s.UltimoDoc,
                              SerieDefeito = s.SerieDefeito
                          }).ToList();


            foreach (var item in res.Departamentos)
            {
                if (mmyObj.DepartamentosSav.Any(x => x.IdDepartamentoSav == item.Id))
                {
                    item.PreSeleccionado = true;
                }
            }

            return res;
        }

        [HttpPost]
        public PagSavPesquisaRes PesquisaSavRes(ViewModels.Savs.PagSavPesquisaParam pesqParams)
        {
            SavsBL encbl = new SavsBL(SessionObject.GetMySessionObject(System.Web.HttpContext.Current));
            PagSavPesquisaRes res = encbl.PesqParamRes(pesqParams);
            return res;
        }
    }
}

