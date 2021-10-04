using Microsoft.Owin;
//using Microsoft.Owin.Security.Facebook;
//using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using NVidrosEncomendas.WebServer.Outros.AutoMapper;
using NVidrosEncomendas.WebServer.Providers;
using Owin;
using System;
using System.Web.Http;

[assembly: OwinStartup(typeof(NVidrosEncomendas.WebServer.Startup))]
namespace NVidrosEncomendas.WebServer
{
    public partial class Startup
    {

        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        //public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        //public static FacebookAuthenticationOptions facebookAuthOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            AutoMapperConfig.RegisterMappings();
            HttpConfiguration config = new HttpConfiguration();
            ConfigureOAuth(app);
            WebApiConfig.Register(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(10),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions() { });

        }
    }
}
