using NfiEncomendas.WebServer.BusinessLogic;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;


namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class EstadosRecolhaController : ApiController
    {
        [HttpGet]
        public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.EstadoRecolha.EstadoRecolha> TabelaEstadoRecolha()
        {
            var res = from c in (new EstadoRecolhaBL()).EstadoRecolhaListaAnulados()
                      where c.NomeEstado != null
                      select new ViewModels.EstadoRecolha.EstadoRecolha
                      {
                          NomeEstado = c.NomeEstado,
                          Cor = c.Cor,
                          Anulado = c.Anulado,
                          EstadoFechaRecolha = c.EstadoFechaRecolha,
                          Id = c.Id

                      };
            return res.OrderBy(x => x.NomeEstado).ToList();
        }

        [HttpGet]
        public ViewModels.EstadoRecolha.PagGestaoEstadoRecolhaEdit EditEstadoRecolha(int id)
        {
            BusinessLogic.EstadoRecolhaBL bl = new BusinessLogic.EstadoRecolhaBL();
            ViewModels.EstadoRecolha.PagGestaoEstadoRecolhaEdit res = new ViewModels.EstadoRecolha.PagGestaoEstadoRecolhaEdit();
            res.EstadoRecolhaParaVM(bl.Ler(id));


            return res;
        }

        [HttpPost]
        public string AtualizaEstadoRecolha(NfiEncomendas.WebServer.Areas.POS.ViewModels.EstadoRecolha.EstadoRecolha EstadoRecolha)
        {
            PermissoesBL.CheckPermissao(PermissoesBL.Permissoes.EstadosRecolha_Editar);

            BusinessLogic.EstadoRecolhaBL bl = new BusinessLogic.EstadoRecolhaBL();
            bl.AtualizaEstadoRecolha(EstadoRecolha.ToModel());
            return "sucesso";
        }
    }
}

