using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class ProdutoSavsBL
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

        public ProdutoSavsBL()
        {
        }
        public ProdutoSavsBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<ProdutoSav> ProdutoSavsListaAnulados()
        {
            return DbContext.ProdutoSav.AsParallel().Where(x => x.NomeProdutoSav != null).OrderBy(x => x.NumProdutoSav);
        }

        public IEnumerable<ProdutoSav> ProdutoSavsLista()
        {
            return DbContext.ProdutoSav.AsParallel().Where(x => x.Anulado == false && x.NomeProdutoSav != null).OrderBy(x => x.NumProdutoSav);
        }

        public ProdutoSav LerProdutoSav(int id)
        {
            var res = DbContext.ProdutoSav.Where(x => x.NumProdutoSav == id).FirstOrDefault();
            if (res == null)
            {
                res = new ProdutoSav();
                res.NumProdutoSav = DbContext.ProdutoSav.Any() ? DbContext.ProdutoSav.Max(x => x.NumProdutoSav) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void AtualizaProdutoSav(ProdutoSav cl)
        {
            try
            {
                ProdutoSav _cl = DbContext.ProdutoSav.Where(x => x.NumProdutoSav == cl.NumProdutoSav).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new ProdutoSav();
                    //_cl.NumProdutoSav = cl.NumProdutoSav;
                }
                _cl.NumProdutoSav = cl.NumProdutoSav;

                //_cl.IdProdutoSav = cl.IdProdutoSav;
                _cl.NomeProdutoSav = cl.NomeProdutoSav;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.ProdutoSav.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = ProdutoSavsListaAnulados().Any() ? ProdutoSavsListaAnulados().Max(x => x.NumProdutoSav) : 0;
            int min = ProdutoSavsListaAnulados().Any() ? ProdutoSavsListaAnulados().Min(x => x.NumProdutoSav) : 0;
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
                    prox = ProdutoSavsListaAnulados().Any() && ProdutoSavsListaAnulados().Where(x => x.NumProdutoSav > id).Any() ? ProdutoSavsListaAnulados().Where(x => x.NumProdutoSav > id).OrderBy(x => x.IdProdutoSav).First().IdProdutoSav : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = ProdutoSavsListaAnulados().Any() && ProdutoSavsListaAnulados().Where(x => x.NumProdutoSav < id).Any() ? ProdutoSavsListaAnulados().Where(x => x.NumProdutoSav < id).OrderByDescending(x => x.IdProdutoSav).First().IdProdutoSav : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }
    }
}