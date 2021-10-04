using NVidrosEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;


namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class EstadoSavController : ApiController
    {
        [HttpGet]
        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.EstadoSav.EstadoSav> TabelaEstadoSav()
        {
            var res = from c in (new EstadoSavBL()).EstadoSavListaAnulados()
                      select new ViewModels.EstadoSav.EstadoSav
                      {
                          Nome = c.NomeEstadoSav,
                          NumEstadoSav = c.IdEstadoSav,
                          Anulado = c.Anulado,
                          SubEstado = c.SubEstado,
                          MarcaEncerrado = c.MarcaEncerrado,
                          PreSeleccionadoNaPesquisa = c.PreSeleccionadoNaPesquisa
                      };
            return res.OrderBy(x => x.NumEstadoSav).ToList();
        }

        [HttpGet]
        public ViewModels.EstadoSav.PagGestaoEstadoSavEdit EditEstadoSav(int id)
        {
            BusinessLogic.EstadoSavBL bl = new BusinessLogic.EstadoSavBL();
            ViewModels.EstadoSav.PagGestaoEstadoSavEdit res = new ViewModels.EstadoSav.PagGestaoEstadoSavEdit();
            res.EstadoSavParaVM(bl.LerEstadoSav(id));

            return res;
        }

        [HttpPost]
        public string AtualizaEstadoSav(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.EstadoSav.EstadoSav EstadoEncomenda)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.EstadosSav_Editar);

            BusinessLogic.EstadoSavBL bl = new BusinessLogic.EstadoSavBL();
            bl.AtualizaEstadoSav(EstadoEncomenda.ToModel());

            return "sucesso";
        }
    }
}

