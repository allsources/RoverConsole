using RoverConsole.Classes;
using RoverConsoleClient.Classes;
using RoverConsoleClient.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace RoverConsoleClient
{
  class Program
  {
    #region "PRIVATE MEMBERS"

    static ConsoleClient _console;

    #endregion "PRIVATE MEMBERS"

    #region "MAIN"

    static void Main(string[] args)
    {
      if (InitializeConsole())
        ProcessConsole();
    }

    #endregion "MAIN"

    #region "CONSOLE CLIENT INITIALIZATION"

    static bool InitializeConsole()
    {
      ConsoleCommands concoleCommands = InitializeCommands();
      if (concoleCommands == null)
        return false;

      ConsoleConnection connection = InitializeConnection();
      if (connection == null)
        return false;

      Console.Write("Initialize console... ");
      _console = new ConsoleClient(concoleCommands, connection);
      Console.WriteLine(_console != null ? "DONE" : "FAIL");

      return _console != null;
    }

    static ConsoleCommands InitializeCommands()
    {
      Console.Write("Initialize collection of commands... ");
      IList<ConsoleCommandConfig> commandConfigs = ResourceHelper.LoadConsoleCommandConfigs();
      var console = new ConsoleCommands(commandConfigs);
      Console.WriteLine(console != null ? "DONE" : "FAIL");
      return console;
    }

    static ConsoleConnection InitializeConnection()
    {
      Console.Write("Initialize connection... ");

      string hubUrl = ConfigurationManager.AppSettings["HubURL"];
      string hubName = ConfigurationManager.AppSettings["HubName"];

      var connection = new ConsoleConnection(hubUrl, hubName);

      Console.WriteLine(connection != null ? "DONE" : "FAIL");

      return connection;
    }

    #endregion "CONSOLE CLIENT INITIALIZATION"

    #region "CONSOLE CLIENT PROCESS"

    static void ProcessConsole()
    {
      _console.Welcome();
      _console.Start();
    }

    #endregion "CONSOLE CLIENT PROCESS"
  }
}
