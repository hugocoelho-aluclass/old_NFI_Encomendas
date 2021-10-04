using System.Web.Mvc;

namespace NfiEncomendas.WebServer.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Index()
        {
            return View();
        }
    }
}