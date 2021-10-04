using NfiEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class SeriesController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Series.Serie> TabelaSeries()
        {
            var res = from c in (new SeriesBL()).SeriesListaAnulados()
                      select new ViewModels.Series.Serie
                      {
                          Nome = c.NomeSerie,
                          NumSerie = c.NumSerie,
                          Inativa = c.Inativa,
                          SerieDefeito = c.SerieDefeito,
                          UltimoDoc = c.UltimoDoc

                      };
            return res.OrderBy(x => x.NumSerie).ToList();
        }

        [HttpGet]
        public ViewModels.Series.PagGestaoSeriesEdit EditSerie(string id)
        {
            BusinessLogic.SeriesBL bl = new BusinessLogic.SeriesBL();
            ViewModels.Series.PagGestaoSeriesEdit res = new ViewModels.Series.PagGestaoSeriesEdit();
            res.SerieParaVM(bl.LerSerie(id));
            KeyValuePair<string, string> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;
            //Console.WriteLine(res.Serie.NumSerie);
            return res;
        }

        [HttpPost]
        public string AtualizaSerie(NfiEncomendas.WebServer.Areas.POS.ViewModels.Series.Serie Serie)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.Series_Editar);

            BusinessLogic.SeriesBL bl = new BusinessLogic.SeriesBL();
            bl.AtualizaSerie(Serie.ToModel());

            return "sucesso";
        }
    }
}
