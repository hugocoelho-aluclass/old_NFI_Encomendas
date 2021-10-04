using NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Estatisticas;
using System.Web.Http;


namespace NVidrosEncomendas.WebServer.Areas.POS.Controllers
{
    public class EstatisticasController : ApiController
    {
        //
        // GET: /Estatisticas/


        //PesquisaEstatisticas
        [HttpPost]
        public Estatisticas PesquisaEstatisticas(NVidrosEncomendas.WebServer.Areas.POS.ViewModels.Estatisticas.PesquisaEstatisticas pesq)
        {
            BusinessLogic.EstatisticasBL bl = new BusinessLogic.EstatisticasBL();
            Estatisticas res = bl.GetEstatisticas(pesq);
            return res;
        }
    }
}