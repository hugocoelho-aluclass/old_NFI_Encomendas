using NfiEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class SetoresEncomendaController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.SetoresEncomenda.SetorEncomenda> TabelaSetores()
        {
            var res = from c in (new SetorEncomendasBL()).SetorListaAnulados()
                      where c.Nome != null
                      select new ViewModels.SetoresEncomenda.SetorEncomenda
                      {
                          Nome = c.Nome,
                          NumSetor = c.NumSetor,
                          Anulado = c.Anulado,
                          Id = c.IdSetorEncomenda
                      };
            return res.OrderBy(x => x.NumSetor).ToList();
        }

        [HttpGet]
        public ViewModels.SetoresEncomenda.PagGestaoSetoresEncomendaEdit EditSetor(int id)
        {
            BusinessLogic.SetorEncomendasBL bl = new BusinessLogic.SetorEncomendasBL();
            ViewModels.SetoresEncomenda.PagGestaoSetoresEncomendaEdit res = new ViewModels.SetoresEncomenda.PagGestaoSetoresEncomendaEdit();
            res.SetorParaVM(bl.LerSetor(id));
            /*   KeyValuePair<int, int> antProx = bl.IdsProximos(id);
               res.idAnt = antProx.Key;
               res.idProx = antProx.Value;
               */
            return res;
        }

        [HttpPost]
        public string AtualizaSetor(NfiEncomendas.WebServer.Areas.POS.ViewModels.SetoresEncomenda.SetorEncomenda setorEncomenda)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.SetoresEncomenda_Editar);

            BusinessLogic.SetorEncomendasBL bl = new BusinessLogic.SetorEncomendasBL();
            bl.AtualizaSetor(setorEncomenda.ToModel());

            return "sucesso";
        }
    }
}

