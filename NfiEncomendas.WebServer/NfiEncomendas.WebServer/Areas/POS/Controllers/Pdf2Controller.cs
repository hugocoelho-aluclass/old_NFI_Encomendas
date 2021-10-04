using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using System.Text;
using System.Web.Mvc;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class Pdf2Controller : Controller
    {
        // GET: POS/Pdf2
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Encomendas()
        {

            return Encs();
            //return new HttpResponseMessage();
        }

        [HttpGet]
        public FileResult EncoPdf()
        {
            //  StreamReader file1 = new StreamReader(HttpRuntime.AppDomainAppPath + "/Content/PdfTemplates/PdfTeste.htm");
            //   string mailTxt = EmailsController.ProcessarMail(file1.ReadToEnd(), context);

            Document document = new Document();
            document.Open();
            MemoryStream ms = new MemoryStream();
            //PdfWriter.GetInstance(document, ms).CloseStream = false;
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            writer.CloseStream = false;



            //iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
            //iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
            // hw.Parse(new StringReader(Encs()));
            StringReader html = new StringReader(Encs());
            XMLWorkerHelper.GetInstance().ParseXHtml(
   writer, document, html
 );

            document.Close();
            //    file1.Close();
            //ShowPdf("Chap0101.pdf");
            byte[] byteInfo = ms.ToArray();
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");

        }

        public string Encs()
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(@"<HTML><HEAD>
<TITLE>Basic HTML Sample Page</TITLE>
</HEAD>
<BODY BGCOLOR='WHITE'>");
            sb.Append(@"<div>
            <table class='table b-t b-light padding-0'>
                            <thead>
                                <tr>
                                    <th>NumDoc</th>
                                    <th>Cliente</th>
                                    <th>Nome Obra</th>
                                    <th>Tipo Encomenda</th>
                                    <th>Data Pedido</th>
                                    <th>Data Entrega</th>
                                    <th style='width:30px;'>S.E.</th>
                                    <th>Fatura</th>
                                    <th>Estado</th>
                                    <th style='width:10px;'></th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- ngRepeat: encomenda in pesqRes.encomendas -->
<tr bindonce='' ng-repeat='encomenda in pesqRes.encomendas' ng-class='classeEstado(encomenda)' class='ng-scope estado-1' bgcolor='#FFFF00' color='#FF0000'>
                                    <td bo-text='encomenda.serieNumEncomenda'>2014/1938</td>
                                    <td bo-text='encomenda.nomeCliente'>2M conseil
</td>
                                    <td bo-text='encomenda.nomeArtigo'>Expo magasin</td>
                                    <td bo-text='encomenda.nomeTipoEncomenda'>Menuiserie
</td>
                                    <td bo-text='encomenda.dataPedidoString'>2014-09-08</td>
                                    <td bo-text='encomenda.dataExpedido'></td>
                                    <td bo-text='encomenda.semanaEntrega'>43</td>
                                    <td bo-text='encomenda.fatura'></td>
                                    <td bo-text='encomenda.estadoDesc'>Em Produção</td>
                                    <td>
                                        <a href='#/app/encomendas/edit/2014/1938' class='btn m-b-xs btn-xs btn-info' ui-sref='app.encomendas.edit({serie: 2014, num:1938})' target='_blank'><i class='fa fa-edit'></i></a>
                                    </td>
                                </tr><!-- end ngRepeat: encomenda in pesqRes.encomendas --><tr bindonce='' ng-repeat='encomenda in pesqRes.encomendas' ng-class='classeEstado(encomenda)' class='ng-scope estado-0'>
                                    <td bo-text='encomenda.serieNumEncomenda'>2014/2296</td>
                                    <td bo-text='encomenda.nomeCliente'>2M conseil
</td>
                                    <td bo-text='encomenda.nomeArtigo'>2axes paris 16e dept 91320 (PEC)</td>
                                    <td bo-text='encomenda.nomeTipoEncomenda'>Menuiserie
</td>
                                    <td bo-text='encomenda.dataPedidoString'>2014-11-18</td>
                                    <td bo-text='encomenda.dataExpedido'></td>
                                    <td bo-text='encomenda.semanaEntrega'>0</td>
                                    <td bo-text='encomenda.fatura'></td>
                                    <td bo-text='encomenda.estadoDesc'>Pendente</td>
                                    <td>
                                        <a href='#/app/encomendas/edit/2014/2296' class='btn m-b-xs btn-xs btn-info' ui-sref='app.encomendas.edit({serie: 2014, num:2296})' target='_blank'><i class='fa fa-edit'></i></a>
                                    </td>
                                </tr><!-- end ngRepeat: encomenda in pesqRes.encomendas --><tr bindonce='' ng-repeat='encomenda in pesqRes.encomendas' ng-class='classeEstado(encomenda)' class='ng-scope estado-0'>
                                    <td bo-text='encomenda.serieNumEncomenda'>2014/2404</td>
                                    <td bo-text='encomenda.nomeCliente'>2M conseil
</td>
                                    <td bo-text='encomenda.nomeArtigo'>Duverneuil</td>
                                    <td bo-text='encomenda.nomeTipoEncomenda'>Menuiserie
</td>
                                    <td bo-text='encomenda.dataPedidoString'>2014-12-10</td>
                                    <td bo-text='encomenda.dataExpedido'></td>
                                    <td bo-text='encomenda.semanaEntrega'>0</td>
                                    <td bo-text='encomenda.fatura'></td>
                                    <td bo-text='encomenda.estadoDesc'>Pendente</td>
                                    <td>
                                        <a href='#/app/encomendas/edit/2014/2404' class='btn m-b-xs btn-xs btn-info' ui-sref='app.encomendas.edit({serie: 2014, num:2404})' target='_blank'><i class='fa fa-edit'></i></a>
                                    </td>
                                </tr><!-- end ngRepeat: encomenda in pesqRes.encomendas -->

                            </tbody>
                            
                        </table> </div>
            ");
            sb.Append("</body></HTML>");
            return sb.ToString();

        }
    }
}