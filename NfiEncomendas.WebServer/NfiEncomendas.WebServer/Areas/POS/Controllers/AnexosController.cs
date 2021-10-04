using NfiEncomendas.WebServer.Areas.POS.ViewModels;
using NfiEncomendas.WebServer.BusinessLogic;
using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    [Authorize]
    public class AnexosController : ApiController
    {
        //[HttpGet]
        //public List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Anexos.Anexo> TabelaAnexo()
        //{
        //    var res = from c in (new AnexosBL()).AnexosListaAnulados()
        //              select new ViewModels.Anexos.Anexo
        //              {
        //                  Nome = c.NomeAnexo,
        //                  NumAnexo = c.NumAnexo,
        //                  Anulado = c.Anulado
        //              };
        //    return res.OrderBy(x => x.NumAnexo).ToList();
        //}

        //[HttpGet]
        //public ViewModels.Anexos.PagGestaoAnexosEdit EditAnexo(int id)
        //{
        //    BusinessLogic.AnexosBL bl = new BusinessLogic.AnexosBL();
        //    ViewModels.Anexos.PagGestaoAnexosEdit res = new ViewModels.Anexos.PagGestaoAnexosEdit();
        //    res.AnexoParaVM(bl.LerAnexo(id));
        //    KeyValuePair<int, int> antProx = bl.IdsProximos(id);
        //    res.idAnt = antProx.Key;
        //    res.idProx = antProx.Value;

        //    return res;
        //}

        //[HttpPost]
        //public string AtualizaAnexo(NfiEncomendas.WebServer.Areas.POS.ViewModels.Anexos.Anexo Anexo)
        //{
        //    BusinessLogic.AnexosBL bl = new BusinessLogic.AnexosBL();
        //    bl.AtualizaAnexo(Anexo.ToModel());

        //    return "sucesso";
        //}

        [HttpPost]
        public HttpResponseMessage InserirAnexo()
        {
            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                string FileName = "";
                int nId = 0;
                var docfiles = new List<string>();
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    FileName = postedFile.FileName;
                    AnexosBL anBl = new AnexosBL();
                    nId = anBl.InserirAnexo(new Anexos() { NomeFicheiro = FileName });
                    var filePath = HttpContext.Current.Server.MapPath("~/Anexos/" + nId);
                    postedFile.SaveAs(filePath);

                    docfiles.Add(filePath);
                }
                result = Request.CreateResponse(HttpStatusCode.OK, new IdNome { Id = nId, Nome = FileName });
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return result;

        }

        [HttpGet]
        public HttpResponseMessage GetAnexo(int id)
        {
            AnexosBL ebl = new AnexosBL();
            Models.Anexos myEnc = ebl.LerAnexo(id);

            MemoryStream ms = new MemoryStream();

            string filename = HttpContext.Current.Server.MapPath("~/Anexos/" + id);

            FileStream fs = new FileStream(filename, FileMode.Open);
            //fs.Close();

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            result.Content = new StreamContent(fs);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = myEnc.NomeFicheiro
            };
            return result;
        }
    }
}

