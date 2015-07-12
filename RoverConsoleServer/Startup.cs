using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.Cookies;
using Owin;
using RoverConsole.Constants;
using RoverConsoleServer.Classes;

[assembly: OwinStartup(typeof(RoverConsoleServer.Startup))]

namespace RoverConsoleServer
{
  public class Startup
  {
    #region "PUBLIC METHODS"

    public void Configuration(IAppBuilder app)
    {
      app.UseCors(CorsOptions.AllowAll);

      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        LoginPath = new PathString(ConsoleConstants.LoginPage),
        LogoutPath = new PathString(ConsoleConstants.LogoutPage),
      });

      app.Map(ConsoleConstants.LoginPage,
        appBuilder => appBuilder.Run(async context => AuthenticationProcessor.Login(context)));

      app.Map(ConsoleConstants.LogoutPage,
        appBuilder => appBuilder.Run(async context => await AuthenticationProcessor.Logout(context)));

      HubConfiguration config = new HubConfiguration();
      config.EnableJSONP = false;
      config.EnableJavaScriptProxies = true;
      config.EnableDetailedErrors = true;

      app.MapSignalR(config);
    }

    #endregion "PUBLIC METHODS"
  }
}
