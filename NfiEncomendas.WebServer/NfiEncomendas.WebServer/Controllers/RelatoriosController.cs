using NfiEncomendas.WebServer.BusinessLogic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NfiEncomendas.WebServer.Controllers
{
    public class RelatoriosController : Controller
    {
        // GET: Relatorios
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Relatorio(string id)
        {

            MemoryStream ms = new MemoryStream();
            RelatoriosBL rb = new RelatoriosBL();
            string filename = rb.PathFicheiro(id);

            FileStream fs = new FileStream(HttpRuntime.AppDomainAppPath + filename, FileMode.Open);

            string contentType = "text/plain";
            if (filename.Split('.').Last().ToLower() == "pdf")
            {
                contentType = "application/pdf";
            }
            //fs.Close();
            return new FileStreamResult(fs, contentType);
        }
    }
}