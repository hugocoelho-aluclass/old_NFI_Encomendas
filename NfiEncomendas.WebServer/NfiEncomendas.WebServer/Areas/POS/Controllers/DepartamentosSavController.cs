using NfiEncomendas.WebServer.BusinessLogic;
using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;


namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class DepartamentoSavsController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.DepartamentoSavs.DepartamentoSav> TabelaDepartamentoSav()
        {
            var res = from c in (new DepartamentoSavsBL()).DepartamentoSavsListaAnulados()
                      select new ViewModels.DepartamentoSavs.DepartamentoSav
                      {
                          Nome = c.NomeDepartamentoSav,
                          NumDepartamentoSav = c.NumDepartamentoSav,
                          Anulado = c.Anulado
                      };
            return res.OrderBy(x => x.NumDepartamentoSav).ToList();
        }

        [HttpGet]
        public ViewModels.DepartamentoSavs.PagGestaoDepartamentoSavsEdit EditDepartamentoSav(int id)
        {
            BusinessLogic.DepartamentoSavsBL bl = new BusinessLogic.DepartamentoSavsBL();
            ViewModels.DepartamentoSavs.PagGestaoDepartamentoSavsEdit res = new ViewModels.DepartamentoSavs.PagGestaoDepartamentoSavsEdit();
            res.DepartamentoSavParaVM(bl.LerDepartamentoSav(id));
            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public string AtualizaDepartamentoSav(NfiEncomendas.WebServer.Areas.POS.ViewModels.DepartamentoSavs.DepartamentoSav DepartamentoSav)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.DepartamentosSav_Editar);

            BusinessLogic.DepartamentoSavsBL bl = new BusinessLogic.DepartamentoSavsBL();
            bl.AtualizaDepartamentoSav(DepartamentoSav.ToModel());

            return "sucesso";
        }
    }
}

