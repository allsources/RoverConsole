using Microsoft.AspNet.SignalR.Client;
using RoverConsole.Classes;
using RoverConsole.Constants;
using RoverConsole.Enums;
using RoverConsole.Loggers;
using System;

namespace RoverConsoleClient.Classes
{
  public class ConsoleClientCommunicator : ConsoleConnection
  {
    #region ".ctor"

    public ConsoleClientCommunicator(string hubUrl, string hubName)
      : base(hubUrl, hubName)
    {
    }

    #endregion ".ctor"

    #region "PUBLIC METHODS"

    public ConsoleConnectionStatus Send(ConsoleCommand command)
    {
      if (Connection.State == ConnectionState.Disconnected)
        return ConsoleConnectionStatus.NoConnection;

      try
      {
        Hub.Invoke(ConsoleConstants.SendCommand, command).Wait();
        return ConsoleConnectionStatus.Ok;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        return ConsoleConnectionStatus.OperationException;
      }
    }

    #endregion "PUBLIC METHODS"
  }
}
