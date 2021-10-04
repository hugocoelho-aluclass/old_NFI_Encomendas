using NVidrosEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class VidrosExtraController : ApiController
    {
        [HttpGet]
        public List<Areas.POS.ViewModels.ExtraVidros.VidrosExtra> Tabela()
        {
            var res = from c in (new VidrosExtraBL()).ListaAnulados()
                      where c.Nome != null
                      select new ViewModels.ExtraVidros.VidrosExtra
                      {
                          Nome = c.Nome,
                          Num = c.Num,
                          Anulado = c.Anulado,
                          Id = c.Id
                      };
            return res.OrderBy(x => x.Num).ToList();
        }

        [HttpGet]
        public ViewModels.ExtraVidros.PagEdit Edit(int id)
        {
            BusinessLogic.VidrosExtraBL bl = new BusinessLogic.VidrosExtraBL();
            
            ViewModels.ExtraVidros.PagEdit res = new ViewModels.ExtraVidros.PagEdit();
            res.ToVm(bl.Ler(id));
          
            return res;
        }

        [HttpPost]
        public string Atualiza(Areas.POS.ViewModels.ExtraVidros.VidrosExtra item)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.VidrosExtra_Editar);

            BusinessLogic.VidrosExtraBL bl = new BusinessLogic.VidrosExtraBL();
            bl.Atualiza(item.ToModel());

            return "sucesso";
        }
    }
}

