using Microsoft.AspNet.SignalR.Client;
using RoverConsole.Classes;
using RoverConsole.Constants;
using RoverConsole.Enums;
using RoverConsole.Loggers;
using System;

namespace Rover.Classes
{
  public class RoverCommunicator : ConsoleConnection
  {
    #region ".ctor"

    public RoverCommunicator(string hubUrl, string hubName)
      : base(hubUrl, hubName)
    {
    }

    #endregion ".ctor"

    #region "PUBLIC METHODS"

    public ConsoleConnectionStatus Response(string responseMsg)
    {
      if (Connection.State == ConnectionState.Disconnected)
        return ConsoleConnectionStatus.NoConnection;

      try
      {
        Hub.Invoke(ConsoleConstants.RoverResponse, responseMsg).Wait();
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
