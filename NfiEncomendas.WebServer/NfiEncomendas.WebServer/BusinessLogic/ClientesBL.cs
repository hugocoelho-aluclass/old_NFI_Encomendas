using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class ClientesBL
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

        public ClientesBL()
        {
        }
        public ClientesBL(AppDbContext db)
        {
            DbContext = db;
        }

        public IEnumerable<Clientes> ClientesListaAnulados()
        {
            return DbContext.Clientes.AsParallel().OrderBy(x => x.IdCliente);
        }

        public IEnumerable<Clientes> ClientesLista()
        {
            return DbContext.Clientes.AsParallel().Where(x => x.Anulado == false && !String.IsNullOrEmpty(x.NomeCliente)).OrderBy(x => x.IdCliente);
        }

        public IEnumerable<Clientes> ClientesListaOrdemNome()
        {
            return DbContext.Clientes.AsParallel().Where(x => x.Anulado == false && !String.IsNullOrEmpty(x.NomeCliente)).OrderBy(x => x.NomeCliente);
        }

        public Clientes LerCliente(int id)
        {
            var res = DbContext.Clientes.Where(x => x.NumCliente == id).FirstOrDefault();
            if (res == null)
            {
                res = new Clientes();
                res.NumCliente = DbContext.Clientes.Any() ? DbContext.Clientes.Max(x => x.NumCliente) + 1 : 1;
                res.Anulado = false;
            }
            return res;

        }

        public void AtualizaCliente(Clientes cl)
        {
            try
            {
                Clientes _cl = DbContext.Clientes.Where(x => x.NumCliente == cl.NumCliente).FirstOrDefault();
                bool novo = false;
                if (_cl == null)
                {
                    novo = true;
                    _cl = new Clientes();
                    _cl.NumCliente = cl.NumCliente;
                }


                _cl.NumCliente = cl.NumCliente;
                _cl.NomeCliente = cl.NomeCliente;
                _cl.Anulado = cl.Anulado;
                if (novo) DbContext.Clientes.Add(_cl);
                DbContext.SaveChanges();
            }
            catch
            {
            }
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = ClientesListaAnulados().Any() ? ClientesListaAnulados().Max(x => x.NumCliente) : 0;
            int min = ClientesListaAnulados().Any() ? ClientesListaAnulados().Min(x => x.NumCliente) : 0;
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
                    prox = ClientesListaAnulados().Any() && ClientesListaAnulados().Where(x => x.NumCliente > id).Any() ? ClientesListaAnulados().Where(x => x.NumCliente > id).OrderBy(x => x.NumCliente).First().NumCliente : 999;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = ClientesListaAnulados().Any() && ClientesListaAnulados().Where(x => x.IdCliente < id).Any() ? ClientesListaAnulados().Where(x => x.NumCliente < id).OrderByDescending(x => x.NumCliente).First().NumCliente : 1;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }
    }
}