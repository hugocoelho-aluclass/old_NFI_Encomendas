using NVidrosEncomendas.WebServer.Models;
using System;

using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class AnexosBL
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

        public AnexosBL()
        {
        }
        public AnexosBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<Anexos> AnexosListaAnulados()
        {
            return DbContext.Anexos.AsParallel().OrderBy(x => x.Id);
        }

        public IEnumerable<Anexos> AnexosLista()
        {
            return DbContext.Anexos.AsParallel().Where(x => x.Anulado == false).OrderBy(x => x.Id);
        }

        public Anexos LerAnexo(int id)
        {
            var res = DbContext.Anexos.Where(x => x.Id == id).FirstOrDefault();
            if (res == null)
            {
                res = new Anexos();
                res.Id = DbContext.Anexos.Any() ? DbContext.Anexos.Max(x => x.Id) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }


        public int InserirAnexo(Anexos cl)
        {
            try
            {
                Anexos _cl = new Anexos();




                //_cl.IdAnexo = cl.IdAnexo;
                _cl.NomeFicheiro = cl.NomeFicheiro;
                _cl.Anulado = cl.Anulado;
                DbContext.Anexos.Add(_cl);
                DbContext.SaveChanges();
                return _cl.Id;
            }
            catch
            {
                throw new Exception("asd");
            }
        }

        public void AtualizaAnexo(Anexos cl)
        {
            try
            {
                Anexos _cl = DbContext.Anexos.Where(x => x.Id == cl.Id).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new Anexos();
                    //_cl.NumAnexo = cl.NumAnexo;
                }


                //_cl.IdAnexo = cl.IdAnexo;
                _cl.NomeFicheiro = cl.NomeFicheiro;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.Anexos.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

    }
}
