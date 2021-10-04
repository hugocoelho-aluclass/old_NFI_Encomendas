//using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NfiEncomendas.WebServer.Models;
using System.Threading.Tasks;
using System.Web.Http;

namespace NfiEncomendas.WebServer.Areas.POS.Controllers
{
    public class ContaController : ApiController
    {

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? null;//HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ContaController()
        {

        }

        [HttpPost]
        [AllowAnonymous]
        public ViewModels.Conta.LoginEstado DoLogin(string username, string password)
        {
            string us = username.ToLower();
            string pw = password;

            ApplicationUser userValidate = UserManager.Find(us, pw);
            if (userValidate != null)
            {
                var task = SignInAsync(userValidate, false);

                task.RunSynchronously();

                return new ViewModels.Conta.LoginEstado() { EstaLogado = true, Sessao = SessionObject.GetMySessionObject(System.Web.HttpContext.Current) };
            }
            else
            {
                //ModelState.AddModelError("", "Invalid username or password.");
                return new ViewModels.Conta.LoginEstado() { EstaLogado = false };
            }

            // ViewBag.ReturnUrl = returnUrl;
            return new ViewModels.Conta.LoginEstado() { EstaLogado = false };
        }


        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager, OAuthDefaults.AuthenticationType));
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return null;// HttpContext.GetOwinContext().Authentication;
            }
        }


    }
}
