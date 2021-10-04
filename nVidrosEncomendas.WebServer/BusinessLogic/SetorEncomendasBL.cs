
using NVidrosEncomendas.WebServer.Models;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class SetorEncomendasBL
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

        public SetorEncomendasBL()
        {
        }
        public SetorEncomendasBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<SetorEncomendas> SetorListaAnulados()
        {
            return DbContext.SetoresEncomendas.AsParallel().OrderBy(x => x.NumSetor);
        }

        public IEnumerable<SetorEncomendas> SetorLista()
        {
            return DbContext.SetoresEncomendas.AsParallel().Where(x => x.Anulado == false && x.Nome != null).OrderBy(x => x.NumSetor);
        }

        public SetorEncomendas LerSetor(int id)
        {
            var res = DbContext.SetoresEncomendas.Where(x => x.IdSetorEncomenda == id).FirstOrDefault();
            if (res == null)
            {
                res = new SetorEncomendas();
                res.NumSetor = DbContext.SetoresEncomendas.Any() ? DbContext.SetoresEncomendas.Max(x => x.NumSetor) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void AtualizaSetor(SetorEncomendas cl)
        {
            try
            {
                SetorEncomendas _cl = DbContext.SetoresEncomendas.Where(x => x.NumSetor == cl.NumSetor).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new SetorEncomendas();
                    //_cl.NumSetor = cl.NumSetor;
                }
                _cl.NumSetor = cl.NumSetor;
                _cl.Nome = cl.Nome;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.SetoresEncomendas.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

    }
}