using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace NVidrosEncomendas.WebServer.Controllers
{
    public class PdfController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Pdf()
        {
            return new HttpResponseMessage();
        }

        [HttpGet]
        public string Encomendas()
        {
            return "String";
            //return new HttpResponseMessage();
        }


        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Pdf4()
        {

            var bytes = System.Text.Encoding.UTF8.GetBytes("");

            using (var input = new MemoryStream(bytes))
            {

                var resp = new HttpResponseMessage()
                {
                    Content = new StreamContent(new MemoryStream())
                };

                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                return resp;
            }
        }

        internal static string ProcessarHtml(string mailTxt, HttpContext context)
        {
            //http://192.168.1.50/Content/logo/trofasoft_simples_peq_v2.png
            mailTxt = mailTxt.Replace("{linkLogo}", ToAbsoluteUrl("~/Content/logo/trofasoft_simples_peq_v2.png", context));
            mailTxt = mailTxt.Replace("{linkLogoPdf}", ToAbsoluteUrl("~/Content/logo/trofasoft_simples_pdf.png", context));
            mailTxt = mailTxt.Replace("{siteUrl}", ToAbsoluteUrl("~/", context));
            mailTxt = mailTxt.Replace("{siteUrlCurto}", "");
            mailTxt = mailTxt.Replace("{nomeEmpresa}", "Trofasoft");
            return mailTxt;
        }

        static string ToAbsoluteUrl(string relativeUrl, HttpContext context)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (context == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = context.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                   url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }


}


