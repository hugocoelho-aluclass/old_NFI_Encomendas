using NVidrosEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class EstadoSavBL
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

        public EstadoSavBL()
        {
        }
        public EstadoSavBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<EstadoSav> EstadoSavListaAnulados()
        {
            return DbContext.EstadosSav.AsParallel().Where(x => x.NomeEstadoSav != null).OrderBy(x => x.IdEstadoSav);
        }

        public IEnumerable<EstadoSav> EstadoSavLista()
        {
            return DbContext.EstadosSav.AsParallel().Where(x => x.Anulado == false && x.NomeEstadoSav != null).OrderBy(x => x.IdEstadoSav);
        }
        public IEnumerable<EstadoSav> EstadoSavListaNaoEncerra()
        {
            return DbContext.EstadosSav.AsParallel().Where(x => x.Anulado == false && x.MarcaEncerrado == false && x.NomeEstadoSav != null).OrderBy(x => x.IdEstadoSav);
        }

        public EstadoSav LerEstadoSav(int id)
        {
            var res = DbContext.EstadosSav.Where(x => x.IdEstadoSav == id).FirstOrDefault();
            if (res == null)
            {
                res = new EstadoSav();
                res.IdEstadoSav = DbContext.EstadosSav.Any() ? DbContext.EstadosSav.Max(x => x.IdEstadoSav) + 1 : 1;
                res.Anulado = false;

            }
            return res;

        }

        public void AtualizaEstadoSav(EstadoSav cl)
        {
            try
            {
                EstadoSav _cl = DbContext.EstadosSav.Where(x => x.IdEstadoSav == cl.IdEstadoSav).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new EstadoSav();
                    //_cl.NumEstadoEncomenda = cl.NumEstadoEncomenda;
                }
                _cl.IdEstadoSav = cl.IdEstadoSav;

                _cl.NomeEstadoSav = cl.NomeEstadoSav;
                _cl.Anulado = cl.Anulado;
                _cl.MarcaEncerrado = cl.MarcaEncerrado;
                _cl.SubEstado = cl.SubEstado;
                _cl.PreSeleccionadoNaPesquisa = cl.PreSeleccionadoNaPesquisa;

                if (novo) DbContext.EstadosSav.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        //public KeyValuePair<int, int> IdsProximos(int id)
        //{
        //    KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
        //    int max = EstadoEncomendaListaAnulados().Any() ? EstadoEncomendaListaAnulados().Max(x => x.NumEstadoEncomenda) : 0;
        //    int min = EstadoEncomendaListaAnulados().Any() ? EstadoEncomendaListaAnulados().Min(x => x.NumEstadoEncomenda) : 0;
        //    if (id == 0)
        //    {
        //        res = new KeyValuePair<int, int>(max, min);
        //    }
        //    else
        //    {
        //        int prox = 0;
        //        int ant = 0;
        //        if (id == max)
        //        {
        //            prox = min;
        //        }
        //        else
        //        {
        //            prox = EstadoEncomendaListaAnulados().Any() && EstadoEncomendaListaAnulados().Where(x => x.NumEstadoEncomenda > id).Any() ? EstadoEncomendaListaAnulados().Where(x => x.NumEstadoEncomenda > id).OrderBy(x => x.IdEstadoSav).First().IdEstadoSav : 999;
        //        }

        //        if (id == min)
        //        {
        //            ant = max;
        //        }
        //        else
        //        {
        //            ant = EstadoEncomendaListaAnulados().Any() && EstadoEncomendaListaAnulados().Where(x => x.NumEstadoEncomenda < id).Any() ? EstadoEncomendaListaAnulados().Where(x => x.NumEstadoEncomenda < id).OrderByDescending(x => x.IdEstadoSav).First().IdEstadoSav : 1;
        //        }
        //        res = new KeyValuePair<int, int>(ant, prox);
        //    }
        //    return res;

        //}
    }
}