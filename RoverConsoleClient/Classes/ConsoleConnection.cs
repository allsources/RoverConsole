using Microsoft.AspNet.SignalR.Client;
using RoverConsole.Classes;
using RoverConsole.Constants;
using RoverConsole.Loggers;
using RoverConsoleClient.Helpers;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace RoverConsoleClient.Classes
{
  public class ConsoleConnection
  {
    #region "PRIVATE MEMBERS"

    private readonly string _hubUrl;

    private readonly string _hubName;
    
    private HubConnection _connection;

    private IHubProxy _hub;

    private Cookie _authCookie;

    #endregion "PRIVATE MEMBERS"

    #region "PUBLIC MEMBERS"

    public bool IsConnected { get { return _connection.State == ConnectionState.Connected; } }

    #endregion "PUBLIC MEMBERS"

    #region ".ctor"

    public ConsoleConnection(string hubUrl, string hubName)
    {
      _hubUrl = hubUrl;
      _hubName = hubName;
      Initialization();
    }

    #endregion ".ctor"

    #region "INITIALIZATION"

    private void Initialization()
    {
      _connection = new HubConnection(_hubUrl);
      _hub = _connection.CreateHubProxy(_hubName);
    }

    #endregion "INITIALIZATION"

    #region "PUBLIC METHODS"

    public string Login(string username, string password)
    {
      if (_authCookie != null)
        return "already login";

      if (AuthenticateUser(username, password))
      {
        _connection.CookieContainer = new CookieContainer();
        _connection.CookieContainer.Add(_authCookie);
      }

      return
        _authCookie != null
          ? "authenticated"
          : "incorrect username";
    }

    public string Logout()
    {
      if (_authCookie == null)
        return "already logout";

      DeAuthenticateUser();

      _connection.CookieContainer = null;

      return "logout";
    }

    public string Connect()
    {
      if (_connection.State == ConnectionState.Connected)
        return "already connected";

      try
      {
        _connection.Start().Wait();
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
        return ex.Message;
      }

      return "connected";
    }

    public string Disconnect()
    {
      if (_connection.State == ConnectionState.Disconnected)
        return "already disconnected";

      try
      {
        _connection.Stop();
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
        return ex.Message;
      }

      return "disconnected";
    }

    public string Reconnect()
    {
      Disconnect();
      Connect();

      return "reconnected";
    }

    public string Send(ConsoleCommand command)
    {
      if (_connection.State == ConnectionState.Disconnected)
        return "No connection";

      try
      {
        _hub.Invoke(ConsoleConstants.SendCommand, command).Wait();
        return "Ok";
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
        return ex.InnerException.Message;
      }
    }

    #endregion "PUBLIC METHODS"

    #region "PRIVATE HELPER METHODS"

    private bool AuthenticateUser(string username, string password)
    {
      _authCookie = null;

      var uri = new Uri(string.Concat(_hubUrl, ConsoleConstants.LoginPage));

      NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
      queryString.Add(ConsoleConstants.Username, username);
      queryString.Add(ConsoleConstants.Password, password);

      HttpWebResponse response = NetworkHelper.Post(uri, queryString.ToString());
      if (response != null && response.Cookies != null && response.Cookies.Count != 0)
      {
        _authCookie = response.Cookies[0];
      }

      return _authCookie != null;
    }

    private bool DeAuthenticateUser()
    {
      var uri = new Uri(string.Concat(_hubUrl, ConsoleConstants.LogoutPage));

      NetworkHelper.Post(uri, string.Empty);

      _authCookie = null;

      return true;
    }

    #endregion "PRIVATE HELPER METHODS"
  }
}
