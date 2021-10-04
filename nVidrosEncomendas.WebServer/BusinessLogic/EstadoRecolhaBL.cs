
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class EstadoRecolhaBL
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

        public EstadoRecolhaBL()
        {
        }
        public EstadoRecolhaBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<EstadoRecolha> EstadoRecolhaListaAnulados()
        {
            return DbContext.EstadoRecolha.AsParallel().OrderBy(x => x.Id);
        }

        public IEnumerable<EstadoRecolha> EstadoRecolhaLista()
        {
            return DbContext.EstadoRecolha.AsParallel().Where(x => x.Anulado == false && x.NomeEstado != null).OrderBy(x => x.Id);
        }

        public EstadoRecolha Ler(int id)
        {
            var res = DbContext.EstadoRecolha.Where(x => x.Id == id).FirstOrDefault();
            if (res == null)
            {
                res = new EstadoRecolha();
                res.Id = DbContext.EstadoRecolha.Any() ? DbContext.EstadoRecolha.Max(x => x.Id) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void AtualizaEstadoRecolha(EstadoRecolha cl)
        {
            try
            {
                EstadoRecolha _cl = DbContext.EstadoRecolha.Where(x => x.Id == cl.Id).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new EstadoRecolha();
                }
                _cl.Id = cl.Id;
                if (cl.NomeEstado == null || cl.NomeEstado.Trim() == "") throw new Exception("campo nome vazio");
                else _cl.NomeEstado = cl.NomeEstado.Trim();
                _cl.Anulado = cl.Anulado;
                _cl.EstadoFechaRecolha = cl.EstadoFechaRecolha;
                _cl.Cor = cl.Cor;
                if (novo) DbContext.EstadoRecolha.Add(_cl);
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}