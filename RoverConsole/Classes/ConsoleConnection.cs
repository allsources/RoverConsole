using Microsoft.AspNet.SignalR.Client;
using RoverConsole.Constants;
using RoverConsole.Enums;
using RoverConsole.Helpers;
using RoverConsole.Loggers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace RoverConsole.Classes
{
  public abstract class ConsoleConnection
  {
    #region "PRIVATE MEMBERS"

    private readonly string _hubUrl;

    private readonly string _hubName;
    
    private Cookie _authCookie;

    #endregion "PRIVATE MEMBERS"

    #region "PROTECTED MEMBERS"

    protected HubConnection Connection;

    protected IHubProxy Hub;

    #endregion "PROTECTED MEMBERS"

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
      Connection = new HubConnection(_hubUrl);
      Hub = Connection.CreateHubProxy(_hubName);
    }

    #endregion "INITIALIZATION"

    #region "PUBLIC METHODS"

    public ConsoleConnectionStatus Login(string token)
    {
      return
        Login(token, null, null);
    }

    public ConsoleConnectionStatus Login(string username, string password)
    {
      return
        Login(null, username, password);
    }

    private ConsoleConnectionStatus Login(string token, string username, string password)
    {
      if (_authCookie != null)
        return ConsoleConnectionStatus.AlreadyAuthenticated;

      if (AuthenticateUser(token, username, password))
      {
        Connection.CookieContainer = new CookieContainer();
        Connection.CookieContainer.Add(_authCookie);
      }

      return
        _authCookie != null
          ? ConsoleConnectionStatus.Authenticated
          : ConsoleConnectionStatus.IncorrectUsername;
    }

    public ConsoleConnectionStatus Logout()
    {
      if (_authCookie == null)
        return ConsoleConnectionStatus.AlreadyLogout;

      DeAuthenticateUser();

      Connection.CookieContainer = null;

      return ConsoleConnectionStatus.Logout;
    }

    public ConsoleConnectionStatus Connect()
    {
      if (Connection.State == ConnectionState.Connected)
        return ConsoleConnectionStatus.AlreadyConnected;

      try
      {
        Connection.Start().Wait();
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
        return ConsoleConnectionStatus.OperationException;
      }

      return ConsoleConnectionStatus.Connected;
    }

    public ConsoleConnectionStatus Disconnect()
    {
      if (Connection.State == ConnectionState.Disconnected)
        return ConsoleConnectionStatus.AlreadyDisconnected;

      try
      {
        Connection.Stop();
      }
      catch(Exception ex)
      {
        Logger.LogException(ex);
        return ConsoleConnectionStatus.OperationException;
      }

      return ConsoleConnectionStatus.Disconnected;
    }

    public ConsoleConnectionStatus Reconnect()
    {
      Disconnect();
      Connect();

      return ConsoleConnectionStatus.Reconnected;
    }

    #endregion "PUBLIC METHODS"

    #region "PRIVATE HELPER METHODS"

    private bool AuthenticateUser(string token, string username, string password)
    {
      _authCookie = null;

      var header = 
        !string.IsNullOrWhiteSpace(token)
          ? new KeyValuePair<string, string>(ConsoleConstants.Token, token)
          : new KeyValuePair<string, string>();

      NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
      if (!string.IsNullOrWhiteSpace(username))
        queryString.Add(ConsoleConstants.Username, username);
      if (!string.IsNullOrWhiteSpace(password))
        queryString.Add(ConsoleConstants.Password, password);

      const bool waitResponse = true;

      HttpWebResponse response = NetworkHelper.Post(GetLoginUrl(), header, queryString.ToString(), waitResponse);
      if (response != null && response.Cookies != null && response.Cookies.Count != 0)
      {
        _authCookie = response.Cookies[0];
      }

      return _authCookie != null;
    }

    private bool DeAuthenticateUser()
    {
      NetworkHelper.Post(GetLoginUrl());

      _authCookie = null;

      return true;
    }

    private Uri GetLoginUrl()
    {
      return
        new Uri(string.Concat(_hubUrl, ConsoleConstants.LoginPage));
    }

    #endregion "PRIVATE HELPER METHODS"
  }
}
