using NVidrosEncomendas.WebServer.BusinessLogic;
using NVidrosEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;


namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class ProdutoSavsController : ApiController
    {
        [HttpGet]
        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.ProdutoSavs.ProdutoSav> TabelaProdutoSav()
        {
            var res = from c in (new ProdutoSavsBL()).ProdutoSavsListaAnulados()
                      select new ViewModels.ProdutoSavs.ProdutoSav
                      {
                          Nome = c.NomeProdutoSav,
                          NumProdutoSav = c.NumProdutoSav,
                          Anulado = c.Anulado
                      };
            return res.OrderBy(x => x.NumProdutoSav).ToList();
        }

        [HttpGet]
        public ViewModels.ProdutoSavs.PagGestaoProdutoSavsEdit EditProdutoSav(int id)
        {

            BusinessLogic.ProdutoSavsBL bl = new BusinessLogic.ProdutoSavsBL();
            ViewModels.ProdutoSavs.PagGestaoProdutoSavsEdit res = new ViewModels.ProdutoSavs.PagGestaoProdutoSavsEdit();
            res.ProdutoSavParaVM(bl.LerProdutoSav(id));
            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public string AtualizaProdutoSav(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.ProdutoSavs.ProdutoSav ProdutoSav)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.ProdutosSav_Editar);

            BusinessLogic.ProdutoSavsBL bl = new BusinessLogic.ProdutoSavsBL();
            bl.AtualizaProdutoSav(ProdutoSav.ToModel());

            return "sucesso";
        }
    }
}

