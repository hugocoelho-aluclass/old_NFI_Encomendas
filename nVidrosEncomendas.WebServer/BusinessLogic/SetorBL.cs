
using NVidrosEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class SetorBL
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

        public SetorBL()
        {
        }
        public SetorBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<Setor> SetorListaAnulados()
        {
            return DbContext.Setores.AsParallel().OrderBy(x => x.NumSetor);
        }

        public IEnumerable<Setor> SetorLista()
        {
            return DbContext.Setores.AsParallel().Where(x => x.Anulado == false && x.Nome != null).OrderBy(x => x.NumSetor);
        }

        public Setor LerSetor(int id)
        {
            var res = DbContext.Setores.Where(x => x.IdSetor == id).FirstOrDefault();
            if (res == null)
            {
                res = new Setor();
                res.NumSetor = DbContext.Setores.Any() ? DbContext.Setores.Max(x => x.NumSetor) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void AtualizaSetor(Setor cl)
        {
            try
            {
                Setor _cl = DbContext.Setores.Where(x => x.NumSetor == cl.NumSetor).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new Setor();
                    //_cl.NumSetor = cl.NumSetor;
                }
                _cl.NumSetor = cl.NumSetor;
                _cl.Nome = cl.Nome;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.Setores.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = SetorListaAnulados().Any() ? SetorListaAnulados().Max(x => x.NumSetor) : 0;
            int min = SetorListaAnulados().Any() ? SetorListaAnulados().Min(x => x.NumSetor) : 0;
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
                    prox = SetorListaAnulados().Any() && SetorListaAnulados().Where(x => x.NumSetor > id).Any() ? SetorListaAnulados().Where(x => x.NumSetor > id).OrderBy(x => x.IdSetor).First().IdSetor : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = SetorListaAnulados().Any() && SetorListaAnulados().Where(x => x.NumSetor < id).Any() ? SetorListaAnulados().Where(x => x.NumSetor < id).OrderByDescending(x => x.IdSetor).First().IdSetor : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }
    }
}