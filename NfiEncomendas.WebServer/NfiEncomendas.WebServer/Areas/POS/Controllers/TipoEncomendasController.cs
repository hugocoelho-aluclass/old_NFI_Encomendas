using NfiEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;


namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class TipoEncomendasController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.TipoEncomendas.TipoEncomenda> TabelaTipoEncomenda()
        {
            var res = from c in (new TipoEncomendasBL()).TipoEncomendasListaAnulados()
                      select new ViewModels.TipoEncomendas.TipoEncomenda
                      {
                          Nome = c.NomeTipoEncomenda,
                          NumTipoEncomenda = c.NumTipoEncomenda,
                          Anulado = c.Anulado,
                          Setor = c.SetorEncomenda != null ? c.SetorEncomenda.Nome : "",
                          SetorId = c.SetorEncomenda != null ? c.SetorEncomenda.IdSetorEncomenda : -1,
                      };
            return res.OrderBy(x => x.NumTipoEncomenda).ToList();
        }

        [HttpGet]
        public ViewModels.TipoEncomendas.PagGestaoTipoEncomendasEdit EditTipoEncomenda(int id)
        {
            BusinessLogic.TipoEncomendasBL bl = new BusinessLogic.TipoEncomendasBL();
            ViewModels.TipoEncomendas.PagGestaoTipoEncomendasEdit res = new ViewModels.TipoEncomendas.PagGestaoTipoEncomendasEdit();
            res.TipoEncomendaParaVM(bl.LerTipoEncomenda(id));
            KeyValuePair<int, int> antProx = bl.IdsProximos(id);
            res.idAnt = antProx.Key;
            res.idProx = antProx.Value;

            return res;
        }

        [HttpPost]
        public string AtualizaTipoEncomenda(NfiEncomendas.WebServer.Areas.POS.ViewModels.TipoEncomendas.TipoEncomenda TipoEncomenda)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.TiposEncomenda_Editar);

            BusinessLogic.TipoEncomendasBL bl = new BusinessLogic.TipoEncomendasBL();
            bl.AtualizaTipoEncomenda(TipoEncomenda.ToModel(bl.DbContext));

            return "sucesso";
        }
    }
}

