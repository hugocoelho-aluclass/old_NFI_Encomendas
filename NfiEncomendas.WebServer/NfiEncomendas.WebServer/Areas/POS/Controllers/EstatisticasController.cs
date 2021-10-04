using NfiEncomendas.WebServer.Areas.POS.ViewModels.Estatisticas;
using System.Web.Http;


namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class EstatisticasController : ApiController
    {
        //
        // GET: /Estatisticas/


        //PesquisaEstatisticas
        [HttpPost]
        public Estatisticas PesquisaEstatisticas(NfiEncomendas.WebServer.Areas.POS.ViewModels.Estatisticas.PesquisaEstatisticas pesq)
        {
            BusinessLogic.EstatisticasBL bl = new BusinessLogic.EstatisticasBL();
            Estatisticas res = bl.GetEstatisticas(pesq);
            return res;
        }
    }
}