using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Data.Entity;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class ProblemaBL
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

        public ProblemaBL()
        {
        }
        public ProblemaBL(AppDbContext db)
        {
            DbContext = db;
        }

        public Problemas LerProblema(int id)
        {
            var res = DbContext.Problemas.Include("Departamento").Where(x => x.IdProblema == id).FirstOrDefault();


            if (res == null)
            {
                res = new Problemas();
                res.IdProblema = DbContext.Problemas.Any() ? DbContext.Problemas.Max(x => x.IdProblema) + 1 : 1;

            }
            return res;

        }


        public int AtualizaProblema(Problemas cl)
        {
            int id;
            try
            {
                Problemas _cl = DbContext.Problemas.Where(x => x.IdProblema == cl.IdProblema).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new Problemas();
                    //_cl.NumTipoEncomenda = cl.NumTipoEncomenda;
                }
                _cl.IdProblema = cl.IdProblema;

                //_cl.IdTipoEncomenda = cl.IdTipoEncomenda;
                _cl.Nome = cl.Nome;
                _cl.Descricao = cl.Descricao;
                _cl.DescricaoCausa = cl.DescricaoCausa;
                _cl.Acompanhamento = cl.Acompanhamento;
                _cl.AcaoImplementar = cl.AcaoImplementar;
                //_cl.DataCriacao = cl.DataCriacao;

                if (novo == true)
                {
                    _cl.DataCriacao = DateTime.Now;
                } else
                {
                    _cl.DataCriacao = cl.DataCriacao;
                }

                _cl.Eficacia = cl.Eficacia;
                _cl.AvaliacaoEficacia = cl.AvaliacaoEficacia;
                if (_cl.Eficacia > 0)
                {
                    _cl.Fechado = true;
                    _cl.DataAvaliacao = DateTime.Now;
                } else
                {
                    _cl.Fechado = false;
                    _cl.DataAvaliacao = cl.DataAvaliacao;
                }

     
                _cl.Departamento = cl.Departamento;

                _cl.IdAnterior = cl.IdAnterior;

                if (novo) DbContext.Problemas.Add(_cl);
                DbContext.SaveChanges();

                id = _cl.IdProblema;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return id;
        }


        public IEnumerable<Problemas> ProblemasListaFechados()
        {
            return DbContext.Problemas.Include("Departamento").AsParallel().OrderBy(x => x.IdProblema);
        }

        public IEnumerable<Problemas> ProblemasLista()
        {
            return DbContext.Problemas.Include("Departamento").AsParallel().Where(x => x.Fechado == false).OrderBy(x => x.IdProblema);
        }


        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = ProblemasListaFechados().Any() ? ProblemasListaFechados().Max(x => x.IdProblema) : 0;
            int min = ProblemasListaFechados().Any() ? ProblemasListaFechados().Min(x => x.IdProblema) : 0;
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
                    prox = ProblemasListaFechados().Any() && ProblemasListaFechados().Where(x => x.IdProblema > id).Any() ? ProblemasListaFechados().Where(x => x.IdProblema > id).OrderBy(x => x.IdProblema).First().IdProblema : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = ProblemasListaFechados().Any() && ProblemasListaFechados().Where(x => x.IdProblema < id).Any() ? ProblemasListaFechados().Where(x => x.IdProblema < id).OrderByDescending(x => x.IdProblema).First().IdProblema : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }

    }
}