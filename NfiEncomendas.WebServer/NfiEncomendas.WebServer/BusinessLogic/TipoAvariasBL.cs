
using NfiEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class TipoAvariasBL
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

        public TipoAvariasBL()
        {
        }
        public TipoAvariasBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<TipoAvarias> TipoAvariasListaAnulados()
        {
            return DbContext.TipoAvarias.AsParallel().OrderBy(x => x.NumTipoAvaria);
        }

        public IEnumerable<TipoAvarias> TipoAvariasLista()
        {
            return DbContext.TipoAvarias.AsParallel().Where(x => x.Anulado == false && x.NomeTipoAvaria != null).OrderBy(x => x.NumTipoAvaria);
        }

        public TipoAvarias LerTipoAvaria(int id)
        {
            var res = DbContext.TipoAvarias.Where(x => x.NumTipoAvaria == id).FirstOrDefault();
            if (res == null)
            {
                res = new TipoAvarias();
                res.NumTipoAvaria = DbContext.TipoAvarias.Any() ? DbContext.TipoAvarias.Max(x => x.NumTipoAvaria) + 1 : 1;
                res.Anulado = false;
                res.InfoExtra = false;
            }
            return res;

        }

        public void AtualizaTipoAvaria(TipoAvarias cl)
        {
            try
            {
                TipoAvarias _cl = DbContext.TipoAvarias.Where(x => x.NumTipoAvaria == cl.NumTipoAvaria).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new TipoAvarias();
                    //_cl.NumTipoAvaria = cl.NumTipoAvaria;
                }
                _cl.NumTipoAvaria = cl.NumTipoAvaria;
                _cl.NomeTipoAvaria = cl.NomeTipoAvaria;
                _cl.Anulado = cl.Anulado;
                _cl.InfoExtra = cl.InfoExtra;
                if (novo) DbContext.TipoAvarias.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = TipoAvariasListaAnulados().Any() ? TipoAvariasListaAnulados().Max(x => x.NumTipoAvaria) : 0;
            int min = TipoAvariasListaAnulados().Any() ? TipoAvariasListaAnulados().Min(x => x.NumTipoAvaria) : 0;
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
                    prox = TipoAvariasListaAnulados().Any() && TipoAvariasListaAnulados().Where(x => x.NumTipoAvaria > id).Any() ? TipoAvariasListaAnulados().Where(x => x.NumTipoAvaria > id).OrderBy(x => x.IdTipoAvaria).First().IdTipoAvaria : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = TipoAvariasListaAnulados().Any() && TipoAvariasListaAnulados().Where(x => x.NumTipoAvaria < id).Any() ? TipoAvariasListaAnulados().Where(x => x.NumTipoAvaria < id).OrderByDescending(x => x.IdTipoAvaria).First().IdTipoAvaria : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }
    }
}