using NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas;
using NfiEncomendas.WebServer.BusinessLogic;
using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using NPOI.POIFS.FileSystem;
using NPOI.HPSF;
using System.Threading.Tasks;
using System.Globalization;



namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class NPOIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage RelatorioProdSemanal()
        {
            //instanciar objeto ebl, para poder consultar BD
            EncomendasBL ebl = new EncomendasBL();

            //Obter lista de tipos de encomenda
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioExcelTipoEncomenda> lista = ebl.RelatorioExcelTipoEncomenda(40, "2021");
            List<NfiEncomendas.WebServer.Areas.POS.ViewModels.Encomendas.RelatorioExcelTipoEncomenda> listaOrdenada = lista.OrderBy(x => x.NumTipoEncomenda).ToList();

            CultureInfo cultureInfo = new CultureInfo("pt-PT");

            var data = DateTime.Now;

            var semana = GetIso8601WeekOfYear(data);

            var primeiro = FirstDateOfWeek(2021, semana, cultureInfo);

            var ultimo = primeiro.AddDays(6).AddHours(23).AddMinutes(59);


            //Criar novo ficheiro xlsx
            IWorkbook workbook = new XSSFWorkbook();
            //Criar lista com as folhas e linhas do ficheiro xlsx 
            List<ISheet> lFolhas = new List<ISheet>();
            List<IRow> lLinhas = new List<IRow>();
            // Adciona à lista, as folhas necessárias
            lFolhas.Add(workbook.CreateSheet("Entrega Priximbattable"));
            lFolhas.Add(workbook.CreateSheet("Entrega Wis Mar NFI"));
            lFolhas.Add(workbook.CreateSheet("Entrega Restantes Clientes"));
            lFolhas.Add(workbook.CreateSheet("Entregas concluídas Priximbattable"));
            lFolhas.Add(workbook.CreateSheet("Entregas concluídas Wis Mar NFI"));
            lFolhas.Add(workbook.CreateSheet("Entregas concluídas Restantes Clientes"));
            lFolhas.Add(workbook.CreateSheet("Produção semanal Priximbattable"));
            lFolhas.Add(workbook.CreateSheet("Produção semanal Wis Mar NFI"));
            lFolhas.Add(workbook.CreateSheet("Produção semanal Restantes Clientes"));
            //Cria um objeto para a celula do excel, onde só é usado para aplicar o estilo algumas celulas, como o cabeçalho
            ICell cell;

            // criar objeto MemoryStream, para guardar um stream de dados em memoria, ou seja, onde vai guardar o ficheiro xlxs criado
            var ms = new MemoryStream();

            // Criar um objeto com o estilo
            var headerStyle = workbook.CreateCellStyle();
            // Define uma borda fina a toda a volta
            headerStyle.BorderTop = BorderStyle.Thin;
            headerStyle.BorderBottom = BorderStyle.Thin;
            headerStyle.BorderLeft = BorderStyle.Thin;
            headerStyle.BorderRight = BorderStyle.Thin;
            // Cria o objeto da fonte, colocando a bold
            var detailSubtotalFont = workbook.CreateFont();
            detailSubtotalFont.IsBold = true;
            headerStyle.SetFont(detailSubtotalFont);

            // Criar um indice para percorrermos as linhas
            var rowIndex = 0;


            
    
            // Criar folhas de Excel
            for (int i = 0; i < lFolhas.Count; i++)
            {
                lLinhas.Add(lFolhas[i].CreateRow(rowIndex));
                //lLinhas[i].CreateCell(0).SetCellValue("Semana/Ano");
            }

            // Criar cabeçalho em todas as folhas
            // O for externo percorre cada um dos tipos de encomenda, já o for interno percorre todas as folhas do excel
            // A cada iteração do tipo de encomenda, são percorridas todas as folhas do ficheiro adicionando o tipod e encomenda ao cabeçalho
            for (int i=0; i < listaOrdenada.Count; i++)
            {
                for (int x = 0; x < lFolhas.Count; x++)
                {
                    cell = lLinhas[x].CreateCell(i + 1);
                    cell.SetCellValue(listaOrdenada[i].NomeTipoEncomenda);
                    cell.CellStyle = headerStyle;
                }
            }
            // Depois de criado o cabeçalho do excel, incrementamos o indice da linha
            rowIndex++;

            
            // Preencher ficheiro excel com os dados das encomendas por tipo de encomenda e semana
            // O ciclo for externo percorre todas as semanas do ano, depois o primeiro ciclo for percorre todas as folhas e o segundo percorre todos o tipos de encomenda
            // A cada iteração da semana, são obtidos os dados dessa semana através de uma query à BD, depois percorremos todas as folhas do excel para adicionar a semana e ano,
            // depois é percorrida toda a lista que obtemos da BD, para adicionar os dados no ficheiro excel, ao percorrermos as lista obtemos o indice das colunas e gravamos em simultaneo em todas as folhas do ficheiro
            // por fim incrementamos o indice das lihnas e passamos para a próxima semana, repetindo todo o processo até percorrermos todas as semanas
            for (int y = 1; y <= 52; y++)
            {
                lista = ebl.RelatorioExcelTipoEncomenda(y, "2021");
                listaOrdenada = lista.OrderBy(x => x.NumTipoEncomenda).ToList();
                
                for (int x = 0; x < lFolhas.Count; x++)
                {
                    lLinhas[x] = lFolhas[x].CreateRow(rowIndex);
                    cell = lLinhas[x].CreateCell(0);
                    cell.SetCellValue(y + "/2021");
                    cell.CellStyle = headerStyle;
                }
   
                for (int i = 0; i < listaOrdenada.Count; i++)
                {
                    lLinhas[0].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalPrix);
                    lLinhas[1].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalWis);
                    lLinhas[2].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalResto);
                    lLinhas[3].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalConcluidoPrix);
                    lLinhas[4].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalConcluidoWis);
                    lLinhas[5].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalConcluidoResto);
                    lLinhas[6].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalProdPrix);
                    lLinhas[7].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalProdWis);
                    lLinhas[8].CreateCell(i + 1).SetCellValue(listaOrdenada[i].totalProdResto);
                }
                rowIndex++;
            }

            // Criamos um objeto temporario da MemoryStream onde guardamos o ficheiro excel, passando para o memorystrem que creamos inicialmento como um Array de bytes, colocamos na o stream na posição zero(inicial)
            // Depois criamos um objeto HttpResponseMessage, que vai servir como resposta(return) para o front end, para o seu conteudo passamos o objeto ms com o ficheiro excel, indicamos o tipo de ficheiro e retornamos o mesmo
            using (MemoryStream tempStream = new MemoryStream())
            {
                workbook.Write(tempStream);
                var byteArray = tempStream.ToArray();
                ms.Write(byteArray, 0, byteArray.Length);
                ms.Position = 0;
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                return response;
            }


        }

        // this method is borrowed from http://stackoverflow.com/a/11155102/284240
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
    }
}