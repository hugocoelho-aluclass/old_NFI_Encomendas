using NfiEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class TipoAvariasController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.TipoAvarias.TipoAvaria> TabelaTipoAvaria()
        {
            var res = from c in (new TipoAvariasBL()).TipoAvariasListaAnulados()
                      where c.NomeTipoAvaria != null
                      select new ViewModels.TipoAvarias.TipoAvaria
                      {
                          Nome = c.NomeTipoAvaria,
                          NumTipoAvaria = c.NumTipoAvaria,
                          Anulado = c.Anulado
                      };
            return res.OrderBy(x => x.NumTipoAvaria).ToList();
        }

        [HttpGet]
        public ViewModels.TipoAvarias.PagGestaoTipoAvariasEdit EditTipoAvaria(int id)
        {
            BusinessLogic.TipoAvariasBL bl = new BusinessLogic.TipoAvariasBL();
            ViewModels.TipoAvarias.PagGestaoTipoAvariasEdit res = new ViewModels.TipoAvarias.PagGestaoTipoAvariasEdit();
            res.TipoAvariaParaVM(bl.LerTipoAvaria(id));
            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public string AtualizaTipoAvaria(NfiEncomendas.WebServer.Areas.POS.ViewModels.TipoAvarias.TipoAvaria TipoAvaria)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.TiposAvaria_Editar);
            BusinessLogic.TipoAvariasBL bl = new BusinessLogic.TipoAvariasBL();
            bl.AtualizaTipoAvaria(TipoAvaria.ToModel());
            return "sucesso";
        }
    }
}