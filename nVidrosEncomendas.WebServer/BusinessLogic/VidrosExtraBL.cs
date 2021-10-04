using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class VidrosExtraBL
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

        public VidrosExtraBL()
        {
        }
        public VidrosExtraBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<VidrosExtra> ListaAnulados()
        {
            return DbContext.VidrosExtra.AsParallel().OrderBy(x => x.Num);
        }

        public IEnumerable<VidrosExtra> Lista()
        {
            return DbContext.VidrosExtra.AsParallel().Where(x => x.Anulado == false && x.Nome != null).OrderBy(x => x.Num);
        }

        public VidrosExtra Ler(int id)
        {
            var res = DbContext.VidrosExtra.Where(x => x.Id == id).FirstOrDefault();
            if (res == null)
            {
                res = new VidrosExtra();
                res.Num = DbContext.VidrosExtra.Any() ? DbContext.VidrosExtra.Max(x => x.Num) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void Atualiza(VidrosExtra cl)
        {
            try
            {
                VidrosExtra _cl = DbContext.VidrosExtra.Where(x => x.Num == cl.Num).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new VidrosExtra();
                    //_cl.Num = cl.Num;
                }
                _cl.Num = cl.Num;
                _cl.Nome = cl.Nome;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.VidrosExtra.Add(_cl);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}