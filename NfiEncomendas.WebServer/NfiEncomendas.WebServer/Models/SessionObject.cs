using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Web;


namespace NfiEncomendas.WebServer.Models
{
    public class SessionObject
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
        public Operadores OperadorObject { get; set; }


        public int? UtilizaodorId { get; set; }
        public string NomeLogin { get; set; }
        public string NomeCompleto { get; set; }

        public string[] Roles { get; set; }

        public bool RoleAdmin { get; set; }
        public bool RoleTecnico { get; set; }

        public SessionObject()
        {
            RoleAdmin = false;
            RoleTecnico = false;
        }

        public static void UpdateSessionObject(HttpContext current)
        {
            using (AppDbContext bd = new AppDbContext())
            {
                string userId = current.User.Identity.GetUserId();
                (current.Session["__MySessionObject"] as SessionObject).NomeLogin = current.User.Identity.Name;
                AuthContext _ctx = new AuthContext();
                UserManager<IdentityUser> _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
                RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));

                var getUser = _userManager.FindByName(current.User.Identity.Name);


                var tmpUser = (from x in bd.Operadores.Include("DepartamentosSav") where x.NomeLogin == current.User.Identity.Name select x).First();


                string[] roles = getUser.Roles.Select(z => z.RoleId).ToArray<string>();
                for (int i = 0; i < roles.Count(); i++)
                {
                    roles[i] = _roleManager.FindById(roles[i]).Name;
                }
                (current.Session["__MySessionObject"] as SessionObject).Roles = roles;

                (current.Session["__MySessionObject"] as SessionObject).RoleAdmin = roles.Any(x => x == Providers.Roles.ADMIN_ROLE);
                (current.Session["__MySessionObject"] as SessionObject).OperadorObject = tmpUser;
            }


            //    (current.Session["__MySessionObject"] as SessionObject).UtilizaodorId = tmpUser.UtilizadorId;
            //    (current.Session["__MySessionObject"] as SessionObject).Username = current.User.Identity.Name;
            //    (current.Session["__MySessionObject"] as SessionObject).Roles = roles;

            //    (current.Session["__MySessionObject"] as SessionObject).RoleAdmin = roles.Contains(StaticRoles.RoleAdmins);
            //    (current.Session["__MySessionObject"] as SessionObject).RoleTecnico = roles.Contains(StaticRoles.RoleTecnicos);

            //    //string[] roles = UserManager.GetRoles(userId);
            //}
        }

        public static SessionObject GetMySessionObject(HttpContext current = null)
        {
            if (current == null) current = HttpContext.Current;
            if ((current.Session["__MySessionObject"] as SessionObject) == null)
            {
                current.Session.Add("__MySessionObject", new SessionObject());
            }

            if ((!(current.Session["__MySessionObject"] as SessionObject).UtilizaodorId.HasValue)
                || (current.Session["__MySessionObject"] as SessionObject).Roles == null)
            {
                UpdateSessionObject(current);
            }

            return current != null ? current.Session["__MySessionObject"] as SessionObject : null;
        }

        public static void LogOff(HttpContext current)
        {
            current.Session["__MySessionObject"] = null;

        }
    }
}