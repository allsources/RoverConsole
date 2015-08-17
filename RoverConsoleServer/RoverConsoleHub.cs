using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using RoverConsole.Classes;
using RoverConsole.Constants;
using RoverConsole.Enums;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace RoverConsoleServer
{
  public class RoverConsoleHub : Hub
  {
    #region "PRIVATE MEMBERS"

    private string _roverConnectionId;

    #endregion "PRIVATE MEMBERS"

    #region "OVERRIDE HUB METHODS"

    public override Task OnConnected()
    {
      BroadcastConsoleMessage(ConsoleConnectionStatus.Connected);
      if (IsRoverConnected())
        SetRoverConnectionId();
      return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
      BroadcastConsoleMessage(ConsoleConnectionStatus.Disconnected);
      return base.OnDisconnected(stopCalled);
    }

    public override Task OnReconnected()
    {
      BroadcastConsoleMessage(ConsoleConnectionStatus.Reconnected);
      return base.OnReconnected();
    }

    #endregion "OVERRIDE HUB METHODS"

    #region "CUSTOM HUB METHODS"

    [CustomAuthorize]
    public void SendCommand(ConsoleCommand command)
    {
      BroadcastConsoleMessage(
        string.Format("command: {0} {1}",
          command.Name.ToString().ToLower(),
          string.Join(" ", command.Arguments)));
      SendCommandToRover(command);
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

    private void BroadcastConsoleMessage(ConsoleConnectionStatus connectionStatus)
    {
      BroadcastConsoleMessage(connectionStatus.ToString());
    }

    private void BroadcastConsoleMessage(string msg)
    {
      var msgToLog = new ConsoleLogMessage(GetUsername(), msg);
      Console.WriteLine("{0} {1}\t{2}", msgToLog.TimeStamp, msgToLog.Username, msgToLog.Text);
      Clients.Others.msgToLog(msgToLog);
    }

    private bool IsRoverConnected()
    {
      return
        GetUsername().Equals(ConsoleConstants.RoverUserName, StringComparison.OrdinalIgnoreCase);
    }

    private void SetRoverConnectionId()
    {
      _roverConnectionId = Context != null ? Context.ConnectionId : string.Empty;
    }

    private void SendCommandToRover(ConsoleCommand command)
    {
      if (!string.IsNullOrWhiteSpace(_roverConnectionId))
        Clients.Client(_roverConnectionId).RoverRequest(command);
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
