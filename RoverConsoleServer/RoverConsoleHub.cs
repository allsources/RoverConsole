using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RoverConsole.Classes;
using RoverConsole.Constants;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RoverConsoleServer
{
  public class RoverConsoleHub : Hub
  {
    #region "OVERRIDE HUB METHODS"

    public override Task OnConnected()
    {
      ProcessConsoleMessage("connected");
      return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      ProcessConsoleMessage("disconnected");
      return base.OnDisconnected(stopCalled);
    }

    public override Task OnReconnected()
    {
      ProcessConsoleMessage("reconnected");
      return base.OnReconnected();
    }

    #endregion "OVERRIDE HUB METHODS"

    #region "CUSTOM HUB METHODS"

    [CustomAuthorize]
    public void SendCommand(ConsoleCommand command)
    {
      ProcessConsoleMessage(string.Format("command: {0} {1}", command.Name, string.Join(" ", command.Arguments)));
    }

    #endregion "CUSTOM HUB METHODS"

    #region "PRIVATE METHODS"

    private string GetUsername()
    {
      return
        Context != null && Context.User != null && Context.User.Identity.IsAuthenticated
          ? Context.User.Identity.Name
          : ConsoleConstants.AnonymousUserName;
    }

    private void ProcessConsoleMessage(string msg)
    {
      Console.WriteLine("{0} {1}\t{2}",
        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
        GetUsername(),
        msg);
    }

    #endregion "PRIVATE METHODS"
  }

  [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
  public class CustomAuthorizeAttribute : AuthorizeAttribute
  {
    protected override bool UserAuthorized(IPrincipal user)
    {
      var principal = user as ClaimsPrincipal;
      return principal != null && principal.Identity.IsAuthenticated;
    }

    public override bool AuthorizeHubConnection(HubDescriptor hubDescriptor, IRequest request)
    {
      return true;
    }
  }
}
