using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class SeriesBL
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

        public SeriesBL()
        {
        }

        public SeriesBL(AppDbContext db)
        {
            DbContext = db;
        }

        public void AtualizaUltimoDocSerie(string serieId)
        {
            Series serie = LerSerie(serieId);
            serie.UltimoDoc = (new EncomendasBL(DbContext)).EncomendasLista().Where(x => x.SerieDoc.NumSerie == serieId).Max(x => x.NumDoc);
            DbContext.SaveChanges();
        }

        public IEnumerable<Series> SeriesListaAnulados()
        {
            return DbContext.Series.AsParallel().OrderBy(x => x.NumSerie);
        }

        public IEnumerable<Series> SeriesLista()
        {
            return DbContext.Series.AsParallel().Where(x => x.Inativa == false).OrderBy(x => x.NumSerie);
        }

        public Series LerSerie(string id)
        {
            var res = DbContext.Series.Where(x => x.NumSerie == id).FirstOrDefault();
            if (res == null)
            {
                res = new Series();
                res.NumSerie = id;
                res.NomeSerie = "";
                res.SerieDefeito = false;
                res.Inativa = false;
            }
            return res;
        }

        public Series LerSerieAno(int id)
        {
            string idString = id.ToString();
            var res = DbContext.Series.Where(x => x.NumSerie == idString).FirstOrDefault();
            if (res == null)
            {
                res = new Series();
                res.NumSerie = idString;
                res.NomeSerie = idString;
                res.SerieDefeito = false;
                res.Inativa = false;
                res.UltimoDoc = 0;
                res.UltimoDocSav = 0;
                AtualizaSerie(res);
            }
            return res;
        }

        public Series SerieDefeito()
        {
            return DbContext.Series.Where(x => x.SerieDefeito).FirstOrDefault();
        }

        public Series ProcuraSerieOuDefeito(string id)
        {
            var res = DbContext.Series.Where(x => x.NumSerie == id).FirstOrDefault();
            if (res == null)
            {
                res = SerieDefeito();
            }

            return res;
        }

        public void AtualizaSerie(Series sr)
        {
            try
            {
                Series _sr = DbContext.Series.Where(x => x.NumSerie == sr.NumSerie).FirstOrDefault();
                bool novo = false;
                if (_sr == null)
                {
                    novo = true;
                    _sr = new Series();
                    _sr.NumSerie = sr.NumSerie;
                }


                _sr.NumSerie = sr.NumSerie;
                _sr.NomeSerie = sr.NomeSerie;
                _sr.Inativa = sr.Inativa;
                _sr.UltimoDoc = sr.UltimoDoc;
                _sr.UltimoDocSav = sr.UltimoDocSav;

                if (sr.SerieDefeito)
                {
                    foreach (var item in DbContext.Series)
                    {
                        item.SerieDefeito = false;
                    }
                }
                _sr.SerieDefeito = sr.SerieDefeito;
                if (novo) DbContext.Series.Add(_sr);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public KeyValuePair<string, string> IdsProximos(string id)
        {
            KeyValuePair<string, string> res = new KeyValuePair<string, string>("", "");
            string max = SeriesListaAnulados().Any() ? SeriesListaAnulados().Last().NumSerie : "";
            string min = SeriesListaAnulados().Any() ? SeriesListaAnulados().Last().NumSerie : "";
            if (id == "")
            {
                res = new KeyValuePair<string, string>(max, min);
            }
            else
            {
                string prox = "";
                string ant = "";
                if (id == max)
                {
                    prox = min;
                }
                else
                {
                    prox = SeriesListaAnulados().Any() && SeriesListaAnulados().Where(x => x.NumSerie.CompareTo(id) > 0).Any() ? SeriesListaAnulados().Where(x => x.NumSerie.CompareTo(id) > 0).OrderBy(x => x.NumSerie).First().NumSerie : "";
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = SeriesListaAnulados().Any() && SeriesListaAnulados().Where(x => x.NumSerie.CompareTo(id) < 0).Any() ? SeriesListaAnulados().Where(x => x.NumSerie.CompareTo(id) < 0).OrderByDescending(x => x.NumSerie).First().NumSerie : "";
                }
                res = new KeyValuePair<string, string>(ant, prox);
            }
            return res;

        }
    }
}