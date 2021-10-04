using System.Web.Mvc;

namespace NfiEncomendas.WebServer.Areas.POS
{
    public class POSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "POS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "POS_default",
                "POS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}