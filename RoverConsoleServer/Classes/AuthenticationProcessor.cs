using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using RoverConsole.Constants;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RoverConsoleServer.Classes
{
  internal static class AuthenticationProcessor
  {
    #region "PUBLIC METHODS"

    public static void Login(IOwinContext context)
    {
      Task<IFormCollection> task = context.Request.ReadFormAsync();
      task.Wait();

      if (task.Result == null)
        return;

      string username = task.Result[ConsoleConstants.Username];
      string password = task.Result[ConsoleConstants.Password];

      if (!ValidateUser(username, password))
        return;

      var claims = new List<Claim>();
      claims.Add(new Claim(ClaimTypes.Name, username));
      var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
      context.Authentication.SignIn(identity);
    }

    public static Task Logout(IOwinContext context)
    {
      context.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      return null;
    }

    #endregion "PUBLIC METHODS"

    #region "PRIVATE METHODS"

    private static bool ValidateUser(string username, string password)
    {
      IList<string> users =
        new List<string>
        {
          "admin",
          "user",
          "driver",
        };

      return
        users.Contains(username) &&
        !string.IsNullOrWhiteSpace(password);
    }

    #endregion "PRIVATE METHODS"
  }
}
