
using NVidrosEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVidrosEncomendas.WebServer.BusinessLogic
{
    public class RecolhaBL
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

        public RecolhaBL()
        {
        }
        public RecolhaBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<Recolhas> RecolhasListaAnulados()
        {
            return DbContext.Recolhas.AsParallel().OrderBy(x => x.Id);
        }

        public IEnumerable<Recolhas> RecolhasLista()
        {
            return DbContext.Recolhas.AsParallel().OrderBy(x => x.Id);
        }

        public Recolhas Ler(int id)
        {
            var res = DbContext.Recolhas.Where(x => x.Id == id).FirstOrDefault();
            if (res == null)
            {
                res = new Recolhas();
                res.Id = DbContext.Recolhas.Any() ? DbContext.Recolhas.Max(x => x.Id) + 1 : 1;

            }
            return res;

        }

        public void AtualizaRecolhas(Recolhas cl)
        {
            try
            {
                Recolhas _cl = DbContext.Recolhas.Where(x => x.Id == cl.Id).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new Recolhas();
                }
                _cl.Id = cl.Id;
                _cl.DataChegadaPrevista = cl.DataChegadaPrevista;
                _cl.DataPedidoRecolha = cl.DataPedidoRecolha;
                _cl.DataRecolha = cl.DataRecolha;
                _cl.RecolhaCompleta = cl.RecolhaCompleta;
                _cl.EstadoProduto = cl.EstadoProduto;
                _cl.EstadoRecolha = DbContext.EstadoRecolha.Where(x => x.Id == cl.EstadoRecolha.Id).FirstOrDefault();
                DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AtualizaRecolhas(Recolhas cl, ref Recolhas res)
        {
            try
            {
                res = DbContext.Recolhas.Where(x => x.Id == cl.Id).FirstOrDefault();
                bool novo = false;
                if (res == null)
                {
                    novo = true;
                    res = new Recolhas();
                }
                res.Id = cl.Id;
                res.DataChegadaPrevista = cl.DataChegadaPrevista;
                res.DataPedidoRecolha = cl.DataPedidoRecolha;
                res.DataRecolha = cl.DataRecolha;
                res.RecolhaCompleta = cl.RecolhaCompleta;
                res.EstadoProduto = cl.EstadoProduto;
                if (cl.EstadoRecolha == null || cl.EstadoRecolha.Id == 0) res.EstadoRecolha = DbContext.EstadoRecolha.FirstOrDefault();
                else res.EstadoRecolha = DbContext.EstadoRecolha.Where(x => x.Id == cl.EstadoRecolha.Id).FirstOrDefault();
                // DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}