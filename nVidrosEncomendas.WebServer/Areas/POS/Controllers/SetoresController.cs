using NVidrosEncomendas.WebServer.BusinessLogic;
using NVidrosEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;


namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class SetoresController : ApiController
    {
        [HttpGet]
        public List<NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Setores.Setor> TabelaSetores()
        {
            var res = from c in (new SetorBL()).SetorListaAnulados()
                      where c.Nome != null
                      select new ViewModels.Setores.Setor
                      {
                          Nome = c.Nome,
                          NumSetor = c.NumSetor,
                          Anulado = c.Anulado,
                          Id = c.IdSetor
                      };
            return res.OrderBy(x => x.NumSetor).ToList();
        }

        [HttpGet]
        public ViewModels.Setores.PagGestaoSetoresEdit EditSetor(int id)
        {
            BusinessLogic.SetorBL bl = new BusinessLogic.SetorBL();
            ViewModels.Setores.PagGestaoSetoresEdit res = new ViewModels.Setores.PagGestaoSetoresEdit();
            res.SetorParaVM(bl.LerSetor(id));
            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public string AtualizaSetor(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Setores.Setor Setor)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.Setores_Editar);

            BusinessLogic.SetorBL bl = new BusinessLogic.SetorBL();
            bl.AtualizaSetor(Setor.ToModel());

            return "sucesso";
        }
    }
}

