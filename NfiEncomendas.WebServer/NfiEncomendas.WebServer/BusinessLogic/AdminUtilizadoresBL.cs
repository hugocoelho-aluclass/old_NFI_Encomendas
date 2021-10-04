using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NfiEncomendas.WebServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NfiEncomendas.WebServer.BusinessLogic
{
    public class AdminUtilizadoresBL
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AdminUtilizadoresBL()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }
        public AdminUtilizadoresBL(AppDbContext dbContext)
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
            DbContext = dbContext;
        }


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

        public string Novo(string nome, string email, string password)
        {
            var user = new IdentityUser() { UserName = nome, Email = email };

            IdentityResult result = _userManager.Create(user, password);

            if (!result.Succeeded)
            {
                return "not ok";
            }

            return "OK";
        }


        public Operadores LerOperador(int id)
        {
            var res = DbContext.Operadores.Include("DepartamentosSav").Where(x => x.UtilizadorId == id).FirstOrDefault();
            if (res == null) res = new Operadores();

            res.Password = "";
            return res;
        }

        public Operadores LerOperador(string id)
        {
            var res = DbContext.Operadores.Where(x => x.NomeLogin == id).FirstOrDefault();
            if (res == null) res = new Operadores();

            res.Password = "";
            return res;
        }

        private Operadores LerOperadorPw(string id)
        {
            var res = DbContext.Operadores.Where(x => x.NomeLogin == id).FirstOrDefault();
            if (res == null) res = new Operadores();

            //res.Password = "";
            return res;
        }

        public KeyValuePair<int, int> IdsProximos(int id)
        {
            KeyValuePair<int, int> res = new KeyValuePair<int, int>(0, 0);
            int max = OperadoresLista().Max(x => x.UtilizadorId);
            int min = OperadoresLista().Min(x => x.UtilizadorId);
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
                    prox = OperadoresLista().Where(x => x.UtilizadorId > id).OrderBy(x => x.UtilizadorId).First().UtilizadorId;
                }

                if (id == min)
                {
                    ant = max;
                }
                else
                {
                    ant = OperadoresLista().Where(x => x.UtilizadorId < id).OrderByDescending(x => x.UtilizadorId).First().UtilizadorId;
                }
                res = new KeyValuePair<int, int>(ant, prox);
            }
            return res;

        }


        public List<Operadores> ListaOperadores()
        {
            List<Operadores> res = new List<Operadores>();
            res = OperadoresLista().ToList();

            return res;
        }

        public IEnumerable<Operadores> OperadoresLista()
        {
            return DbContext.Operadores.Where(x => x.NomeLogin != "MasterAdmin" && !x.Anulado);
        }


        public IEnumerable<Operadores> OperadoresListaSav()
        {
            return DbContext.Operadores.Where(x => x.NomeLogin != "MasterAdmin" && !x.Anulado && x.Sav);
        }

        public IEnumerable<Operadores> OperadoresListaMasterIncluido()
        {
            return DbContext.Operadores.Include("DepartamentosSav").Where(x => !x.Anulado);
        }


        public void AtualizaOperador(Operadores op)
        {
            try
            {
                Operadores _op = DbContext.Operadores.Include("DepartamentosSav").Where(x => x.UtilizadorId == op.UtilizadorId).FirstOrDefault();
                DepartamentoSavsBL depBl = new DepartamentoSavsBL(this.DbContext);
                if (_op == null)
                {
                    _op = new Operadores();
                }

                //_op.EditadoPor = SessionObject.GetMySessionObject(HttpContext.Current).UtilizaodorId;
                _op.Admin = op.Admin;
                _op.Ativo = op.Ativo;
                _op.CriadoData = DateTime.Now;
                _op.CriadoPor = DbContext.Operadores.First();
                _op.EditadoData = DateTime.Now;
                _op.EditadoPor = DbContext.Operadores.First();
                _op.NomeCompleto = op.NomeCompleto;
                _op.NomeLogin = op.NomeLogin;
                _op.Email = op.NomeLogin + "@local";
                _op.ImagemAvatar = op.ImagemAvatar;
                _op.Anulado = op.Anulado;
                _op.Sav = op.Sav;
                _op.AdminSav = op.AdminSav;
                _op.Comercial = op.Comercial;
                //var todosDep = _op.DepartamentosSav;

                //todosDep.ForEach(cs => _op.DepartamentosSav.Remove(cs));
                _op.DepartamentosSav.Clear();

                DbContext.SaveChanges();
                var todosDep = depBl.DepartamentoSavsLista();
                foreach (var item in op.DepartamentosSav)
                {
                    var d = todosDep.Where(x => x.IdDepartamentoSav == item.IdDepartamentoSav).First();
                    _op.DepartamentosSav.Add(d);
                }

                if (op.UtilizadorId == 0 || op.UtilizadorId == -1)
                {
                    _op.Password = op.Password;
                    DbContext.Operadores.Add(_op);
                }
                else { }
                DbContext.SaveChanges();
                AtualizarMembershipOperador(_op);

                _op.AspIdentityId = LerIdentityId(op.NomeLogin);
                DbContext.SaveChanges();
                CheckOperadoresRepetidos();
            }
            catch (Exception ex)
            {
                string a = ex.Message;

            }
        }

        public void CheckOperadoresRepetidos()
        {
            List<Operadores> todosOperadores = DbContext.Operadores.OrderBy(x => x.UtilizadorId).ToList();
            List<Operadores> operadosAmanter = new List<Operadores>();
            List<Operadores> operadosAremover = new List<Operadores>();

            foreach (var item in todosOperadores)
            {
                if (!operadosAmanter.Any(x => x.AspIdentityId == item.AspIdentityId))
                {
                    operadosAmanter.Add(item);
                }
                else
                {
                    operadosAremover.Add(item);
                }
            }

            if (operadosAremover.Count != 0)
            {
                foreach (var item in operadosAremover)
                {
                    List<Savs> savs = DbContext.Savs.Include("CriadoPor").Include("EditadoPor").Where(x => x.CriadoPor.UtilizadorId == item.UtilizadorId || x.EditadoPor.UtilizadorId == item.UtilizadorId).ToList();
                    if (savs.Count() != 0)
                    {
                        foreach (var sav in savs)
                        {
                            sav.CriadoPor = operadosAmanter.First(x => x.AspIdentityId == sav.CriadoPor.AspIdentityId);
                            sav.EditadoPor = operadosAmanter.First(x => x.AspIdentityId == sav.EditadoPor.AspIdentityId);
                        }
                        DbContext.SaveChanges();
                    }
                }

                foreach (var item in operadosAremover)
                {
                    try
                    {
                        DbContext.Operadores.Remove(item);
                        DbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }

        public void AtualizarPassword(int operadorId, string password)
        {
            Operadores op = this.LerOperador(operadorId);
            op.Password = password;
            AtualizarMembershipOperador(op);
            DbContext.SaveChanges();
        }

        public string AtualizarPassword(string operadorId, string passwordAtual, string passwordNova)
        {
            Operadores op = this.LerOperadorPw(operadorId);
            if (op.Password != passwordAtual) return "pwErrada";
            op.Password = passwordNova;
            AtualizarMembershipOperador(op);
            DbContext.SaveChanges();
            return "sucesso";
        }

        public void AtualizarMembershipOperador(Operadores op)
        {
            IdentityUser userGet = _userManager.FindByName(op.NomeLogin);

            if (userGet == null)
            {
                var result = _userManager.Create(new IdentityUser() { UserName = op.NomeLogin, Email = op.Email }, op.Password);
                userGet = _userManager.FindByName(op.NomeLogin);
            }

            if (!op.Ativo || !op.Anulado)
            {
                userGet.LockoutEnabled = true;
            }

            IdentityRole roleUser = _roleManager.FindByName("USER");
            if (!userGet.Roles.Any(x => x.RoleId == roleUser.Id))
            {
                _userManager.AddToRole(userGet.Id, "USER");
                //userGet.Roles.Add(new IdentityUserRole() { RoleId = roleAdmin.Id, UserId = userGet.Id });
            }

            if (op.Admin)
            {
                IdentityRole roleAdmin = _roleManager.FindByName("ADMIN");
                if (!userGet.Roles.Any(x => x.RoleId == roleAdmin.Id))
                {
                    _userManager.AddToRole(userGet.Id, "ADMIN");
                    //userGet.Roles.Add(new IdentityUserRole() { RoleId = roleAdmin.Id, UserId = userGet.Id });
                }
            }
            else
            {
                IdentityRole roleAdmin = _roleManager.FindByName("ADMIN");
                if (userGet.Roles.Any(x => x.RoleId == roleAdmin.Id))
                {
                    _userManager.RemoveFromRole(userGet.Id, "ADMIN");
                    //userGet.Roles.Add(new IdentityUserRole() { RoleId = roleAdmin.Id, UserId = userGet.Id });
                }
            }

            _userManager.RemovePassword(userGet.Id);
            _userManager.AddPassword(userGet.Id, op.Password);

        }

        public string LerIdentityId(string nomeLogin)
        {
            return _userManager.FindByName(nomeLogin).Id;

        }

        public void GarantirMasterAdmin()
        {
            //MasterADmin
            //Trofa2014 = "f32144e32aea4b83138df62f154246ca"
            string username = "MasterAdmin";
            string password = "f32144e32aea4b83138df62f154246ca";
            GarantirRoles();

            var user = new IdentityUser() { UserName = username, Email = username + "@localhost" };

            IdentityUser userGet = _userManager.FindByName(username);

            if (userGet == null)
            {
                var result = _userManager.Create(user, password);
            }
            else
            {
                _userManager.RemovePassword(userGet.Id);
                _userManager.AddPassword(userGet.Id, password);
            }
            userGet = _userManager.FindByName(username);
            IdentityRole roleAdmin = _roleManager.FindByName("ADMIN");
            if (!userGet.Roles.Any(x => x.RoleId == roleAdmin.Id))
            {
                _userManager.AddToRole(userGet.Id, "ADMIN");
                //userGet.Roles.Add(new Identity.Include("EditadoPor")UserRole() { RoleId = roleAdmin.Id, UserId = userGet.Id });
            }


        }

        public void GarantirRoles()
        {
            string[] rolesDefault = { "USER", "ADMIN" };


            foreach (var roleName in rolesDefault)
            {
                IdentityRole roleGet = _roleManager.FindByName(roleName);

                if (roleGet == null)
                {
                    var result = _roleManager.Create(new IdentityRole(roleName));
                }

            }


        }

        public void AtualizaMembership()
        {

        }



    }
}