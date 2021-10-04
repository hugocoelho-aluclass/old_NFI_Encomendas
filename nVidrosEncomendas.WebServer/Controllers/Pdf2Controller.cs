using System.Web.Mvc;

namespace NVidrosEncomendas.WebServer.Controllers
{
    public class Pdf2Controller : Controller
    {
        // GET: Pdf2
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Encomendas()
        {
            return "String";
            //return new HttpResponseMessage();
        }
    }
}