using AutoMapper;
using System.Collections.Generic;

namespace NfiEncomendas.WebServer.Areas.POS.ViewModels.Admin
{
    public class Operador
    {
        public Operador()
        {
            IdOperador = 0;
            Nome = "";
            NomeCompleto = "";
            Email = "";
            Admin = false;
            Ativo = true;
            Anulado = false;
        }

        public int IdOperador { get; set; }
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public bool Admin { get; set; }
        public bool Anulado { get; set; }
        public bool Ativo { get; set; }
        public string Password { get; set; }
        public string ConfirmaPassword { get; set; }
        public bool Sav { get; set; }
        public bool AdminSav { get; set; }
        public bool Comercial { get; set; }

        public List<IdNome> Departamentos { get; set; }


        public Models.Operadores ToModel()
        {
            NfiEncomendas.WebServer.BusinessLogic.AdminUtilizadoresBL admBl = new NfiEncomendas.WebServer.BusinessLogic.AdminUtilizadoresBL();
            NfiEncomendas.WebServer.BusinessLogic.DepartamentoSavsBL depSavsBl = new BusinessLogic.DepartamentoSavsBL(admBl.DbContext);

            Models.Operadores res = admBl.LerOperador(this.IdOperador); //new Models.Operadores();

            res.UtilizadorId = this.IdOperador;
            res.NomeLogin = this.Nome;
            res.NomeCompleto = this.NomeCompleto;
            res.Admin = this.Admin;
            res.Ativo = this.Ativo;
            res.Email = this.Email;
            res.Password = this.Password;
            res.Anulado = this.Anulado;
            res.Sav = this.Sav;
            res.AdminSav = this.AdminSav;
            res.Comercial = this.Comercial;

            res.DepartamentosSav.Clear();
            foreach (var item in this.Departamentos)
            {
                res.DepartamentosSav.Add(depSavsBl.LerDepartamentoSav(item.Id));
            }

            return res;
        }

    }

    public class PagGestaoOperadores
    {
        public List<Operador> Operadores { get; set; }

        public PagGestaoOperadores()
        {
            Operadores = new List<Operador>();
        }

        public void OperadoresParaVM(List<Models.Operadores> op)
        {
            foreach (Models.Operadores item in op)
            {
                Operador tmp = new Operador();
                tmp.Nome = item.NomeLogin;
                tmp.NomeCompleto = item.NomeCompleto;
                tmp.IdOperador = item.UtilizadorId;
                tmp.Admin = item.Admin;
                tmp.Ativo = item.Ativo;
                tmp.Sav = item.Sav;
                tmp.AdminSav = item.AdminSav;
                tmp.Comercial = item.Comercial;

                Operadores.Add(tmp);
            }
        }
    }


    public class OperadorEdit
    {
        public int IdOperador { get; set; }
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public bool Admin { get; set; }
        public bool Ativo { get; set; }
        public bool Anulado { get; set; }
        public string Email { get; set; }
        public bool AdminSav { get; set; }
        public bool Comercial { get; set; }

        public bool Sav { get; set; }
        public ICollection<IdNome> Departamentos { get; set; }
    }
    public class PagGestaoOperadoresEdit
    {
        public OperadorEdit Operador { get; set; }
        public int idProx { get; set; }
        public int idAnt { get; set; }

        public List<IdNome> Departamentos { get; set; }

        public PagGestaoOperadoresEdit()
        {
            Operador = new OperadorEdit();
            idProx = 999;
            idAnt = 0;
        }

        public void OperadorParaVM(Models.Operadores item)
        {
            Operador.IdOperador = item.UtilizadorId;
            Operador.Nome = item.NomeLogin;
            Operador.NomeCompleto = item.NomeCompleto;

            Operador.Admin = item.Admin;
            Operador.Ativo = item.Ativo;
            Operador.Anulado = item.Anulado;
            Operador.Email = item.Email;
            Operador.AdminSav = item.AdminSav;
            Operador.Comercial = item.Comercial;

            Operador.Sav = item.Sav;
            Operador.Departamentos = Mapper.Map<ICollection<Models.DepartamentoSav>, ICollection<ViewModels.IdNome>>(item.DepartamentosSav);

        }
    }

    public class PagGestaoOperadoresAtualizaPassword
    {
        public int OperadorId { get; set; }
        public string Md5Password { get; set; }
        public PagGestaoOperadoresAtualizaPassword()
        {

        }
    }
}