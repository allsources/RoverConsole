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
      string username;
      if (AuthenticateByToken(context, out username) ||
          AuthenticateByCredentials(context, out username))
        CreateAuthCookie(context, username);
    }

    public static Task Logout(IOwinContext context)
    {
      context.Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      return null;
    }

    #endregion "PUBLIC METHODS"

    #region "AUTHENTICATE BY TOKEN"

    private static bool AuthenticateByToken(IOwinContext context, out string username)
    {
      username = null;

      string token = GetToken(context);

      return
        !string.IsNullOrWhiteSpace(token) &&
        ValidateUser(token, out username);
    }

    private static string GetToken(IOwinContext context)
    {
      return
        context.Request.Headers != null
          ? context.Request.Headers[ConsoleConstants.Token]
          : null;
    }
    
    private static bool ValidateUser(string token, out string username)
    {
      var tokens =
        new Dictionary<string, string>
        {
          { "1234567890", ConsoleConstants.RoverUserName },
        };

      username = tokens[token];

      return
        tokens.ContainsKey(token);
    }

    #endregion "AUTHENTICATE BY TOKEN"

    #region "AUTHENTICATE BY CREDENTIALS"

    private static bool AuthenticateByCredentials(IOwinContext context, out string username)
    {
      string password;
      GetCredentials(context, out username, out password);

      return
        !string.IsNullOrWhiteSpace(username) &&
        ValidateUser(username, password);
    }

    private static void GetCredentials(IOwinContext context, out string username, out string password)
    {
      username = password = null;

      Task<IFormCollection> task = context.Request.ReadFormAsync();
      task.Wait();

      if (task.Result == null)
        return;

      username = task.Result[ConsoleConstants.Username];
      password = task.Result[ConsoleConstants.Password];
    }

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

    #endregion "AUTHENTICATE BY CREDENTIALS"

    #region "PRIVATE METHODS"

    private static void CreateAuthCookie(IOwinContext context, string username)
    {
      var claims = new List<Claim>();
      claims.Add(new Claim(ClaimTypes.Name, username));
      var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
      context.Authentication.SignIn(identity);
    }

    #endregion "PRIVATE METHODS"
  }
}
