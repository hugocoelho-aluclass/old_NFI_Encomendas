using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class DepartamentoSavsBL
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

        public DepartamentoSavsBL()
        {
        }
        public DepartamentoSavsBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<DepartamentoSav> DepartamentoSavsListaAnulados()
        {
            return DbContext.DepartamentoSav.AsParallel().Where(x => x.NomeDepartamentoSav != null).OrderBy(x => x.NumDepartamentoSav);
        }

        public IEnumerable<DepartamentoSav> DepartamentoSavsLista()
        {
            return DbContext.DepartamentoSav.AsParallel().Where(x => x.Anulado == false && x.NomeDepartamentoSav != null).OrderBy(x => x.NumDepartamentoSav);
        }



        public DepartamentoSav LerDepartamentoSav(int id)
        {
            var res = DbContext.DepartamentoSav.Where(x => x.NumDepartamentoSav == id).FirstOrDefault();
            if (res == null)
            {
                res = new DepartamentoSav();
                res.NumDepartamentoSav = DbContext.DepartamentoSav.Any() ? DbContext.DepartamentoSav.Max(x => x.NumDepartamentoSav) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void AtualizaDepartamentoSav(DepartamentoSav cl)
        {
            try
            {
                DepartamentoSav _cl = DbContext.DepartamentoSav.Where(x => x.NumDepartamentoSav == cl.NumDepartamentoSav).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new DepartamentoSav();
                    //_cl.NumDepartamentoSav = cl.NumDepartamentoSav;
                }
                _cl.NumDepartamentoSav = cl.NumDepartamentoSav;

                //_cl.IdDepartamentoSav = cl.IdDepartamentoSav;
                _cl.NomeDepartamentoSav = cl.NomeDepartamentoSav;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.DepartamentoSav.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = DepartamentoSavsListaAnulados().Any() ? DepartamentoSavsListaAnulados().Max(x => x.NumDepartamentoSav) : 0;
            int min = DepartamentoSavsListaAnulados().Any() ? DepartamentoSavsListaAnulados().Min(x => x.NumDepartamentoSav) : 0;
            if (id == 0)
            {
                res = new KeyValuePair<int, int>(max, min);
            }
            else
            {
                int prox = 0;
                int ant = 0;
                if (id == max)
                {
                    prox = min;
                }
                else
                {
                    prox = DepartamentoSavsListaAnulados().Any() && DepartamentoSavsListaAnulados().Where(x => x.NumDepartamentoSav > id).Any() ? DepartamentoSavsListaAnulados().Where(x => x.NumDepartamentoSav > id).OrderBy(x => x.IdDepartamentoSav).First().IdDepartamentoSav : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = DepartamentoSavsListaAnulados().Any() && DepartamentoSavsListaAnulados().Where(x => x.NumDepartamentoSav < id).Any() ? DepartamentoSavsListaAnulados().Where(x => x.NumDepartamentoSav < id).OrderByDescending(x => x.IdDepartamentoSav).First().IdDepartamentoSav : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }
    }
}