using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;


namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class TipoEncomendasBL
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

        public TipoEncomendasBL()
        {
        }
        public TipoEncomendasBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<TipoEncomendas> TipoEncomendasListaAnulados()
        {
            return DbContext.TipoEncomendas.Include(x => x.SetorEncomenda).AsParallel().OrderBy(x => x.NumTipoEncomenda);
        }

        public IEnumerable<TipoEncomendas> TipoEncomendasLista()
        {
            return DbContext.TipoEncomendas.Include(x => x.SetorEncomenda).AsParallel().Where(x => x.Anulado == false).OrderBy(x => x.NumTipoEncomenda);
        }

        public TipoEncomendas LerTipoEncomenda(int id)
        {
            var res = DbContext.TipoEncomendas.Include(x => x.SetorEncomenda).Where(x => x.NumTipoEncomenda == id).FirstOrDefault();
            if (res == null)
            {
                res = new TipoEncomendas();
                res.NumTipoEncomenda = DbContext.TipoEncomendas.Any() ? DbContext.TipoEncomendas.Max(x => x.NumTipoEncomenda) + 1 : 1;
                res.Anulado = false;

            }
            return res;

        }

        public void AtualizaTipoEncomenda(TipoEncomendas cl)
        {
            try
            {
                TipoEncomendas _cl = DbContext.TipoEncomendas.Where(x => x.NumTipoEncomenda == cl.NumTipoEncomenda).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new TipoEncomendas();
                    //_cl.NumTipoEncomenda = cl.NumTipoEncomenda;
                }
                _cl.NumTipoEncomenda = cl.NumTipoEncomenda;

                //_cl.IdTipoEncomenda = cl.IdTipoEncomenda;
                _cl.NomeTipoEncomenda = cl.NomeTipoEncomenda;
                _cl.Anulado = cl.Anulado;
                _cl.SetorEncomenda = DbContext.SetoresEncomendas.Where(x => x.IdSetorEncomenda == cl.SetorEncomenda.IdSetorEncomenda).FirstOrDefault();

                if (novo) DbContext.TipoEncomendas.Add(_cl);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = TipoEncomendasListaAnulados().Any() ? TipoEncomendasListaAnulados().Max(x => x.NumTipoEncomenda) : 0;
            int min = TipoEncomendasListaAnulados().Any() ? TipoEncomendasListaAnulados().Min(x => x.NumTipoEncomenda) : 0;
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
                    prox = TipoEncomendasListaAnulados().Any() && TipoEncomendasListaAnulados().Where(x => x.NumTipoEncomenda > id).Any() ? TipoEncomendasListaAnulados().Where(x => x.NumTipoEncomenda > id).OrderBy(x => x.IdTipoEncomenda).First().IdTipoEncomenda : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = TipoEncomendasListaAnulados().Any() && TipoEncomendasListaAnulados().Where(x => x.NumTipoEncomenda < id).Any() ? TipoEncomendasListaAnulados().Where(x => x.NumTipoEncomenda < id).OrderByDescending(x => x.IdTipoEncomenda).First().IdTipoEncomenda : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }
    }
}