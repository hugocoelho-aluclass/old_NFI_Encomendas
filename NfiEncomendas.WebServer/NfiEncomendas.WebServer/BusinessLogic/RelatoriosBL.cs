using NfiEncomendas.WebServer.Areas.POS.ViewModels;
using NfiEncomendas.WebServer.BusinessLogic;
using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Globalization;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class RelatoriosBL
    {
        private AppDbContext _dbContext;
        public AppDbContext DbContext
        {
            get
            {
                if (_dbContext == null) _dbContext = new AppDbContext();
                return _dbContext;
            }
            set { _dbContext = value; }
        }


        public string PathFicheiro(string uid)
        {
            DateTime dataLimite = DateTime.Now.AddMinutes(-10);
            //TimeSpan ts = new TimeSpan(0, 10, 0);
            var rblRes = DbContext.Relatorios.Where(x => x.UniqueId == uid);
            if (rblRes.Count() == 0 || rblRes.First().DataGerado < dataLimite)
            {

                return "/Reports/notfound.html";
            }

            return rblRes.First().NomeFicheiro;
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            CultureInfo ci = new CultureInfo("pt-PT");
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
      

        public List<Areas.POS.ViewModels.Relatorios.RelatorioProdutoSav> tabelaSav(string a, int s)
        {

            int ano = 0;
            Int32.TryParse(a, out ano);
            DateTime idata = BusinessLogic.RelatoriosBL.FirstDateOfWeek(ano, s);
            DateTime fdata = idata.AddDays(6).AddHours(23).AddMinutes(59);


            var i = idata.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var f = fdata.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var sql = "select * from RelatorioSav('" + i + "', '" + f + "')";

            var savs = DbContext.Database.SqlQuery<NfiEncomendas.WebServer.Areas.POS.ViewModels.Relatorios.RelatorioProdutoSav>(sql).ToList();

            return savs;
        }
    }
}