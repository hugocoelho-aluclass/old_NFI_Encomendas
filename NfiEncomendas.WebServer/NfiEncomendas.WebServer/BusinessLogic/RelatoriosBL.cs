using NfiEncomendas.WebServer.Models;
using System;
using System.Linq;

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
    }
}