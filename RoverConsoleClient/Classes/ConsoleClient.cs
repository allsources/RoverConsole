using RoverConsole.Classes;
using RoverConsole.Enums;
using System;
using System.Collections.Generic;

namespace RoverConsoleClient.Classes
{
  public class ConsoleClient
  {
    #region "PRIVATE MEMBERS"

    private ConsoleConnection _connection;

    private ConsoleCommand _command;

    private ConsoleCommands _commands;

    private bool _inProcess = true;

    private bool IsCommandValid { get { return _command.ValidationStatus == CommandValidationStatus.Ok; } }

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public ConsoleClient(ConsoleCommands commands, ConsoleConnection connection)
    {
      _commands = commands;
      _connection = connection;
    }

    #endregion ".ctor"

    #region "INITIALIZATION"

    #endregion "INITIALIZATION"

    #region "PROCESS"

    public void Start()
    {
      Login();
      Process();
    }

    private void Process()
    {
      while(_inProcess)
      {
        ReadCommand();
        if (IsCommandValid)
          ExecuteCommand();
        else
          Console.WriteLine(_command.ValidationStatus);
      }
    }

    public void Welcome()
    {
      Console.WriteLine("\nHello, and Welcome to the Rover console!");
    }

    #endregion "PROCESS"

    #region "PARSE COMMAND"

    private void ReadCommand()
    {
      string commandRawData = ReadCommandRawData();
      _command = _commands.Parse(commandRawData);
    }

    private string ReadCommandRawData()
    {
      Console.Write("\n> ");
      return Console.ReadLine();
    }

    #endregion "PARSE COMMAND"

    #region "EXECUTE COMMAND"

    private void ExecuteCommand()
    {
      if (_command.Call == CommandCall.Internal)
        ExecuteInternal();
      else
        ExecuteExternal();
    }

    private void ExecuteInternal()
    {
      switch(_command.Name)
      {
        case CommandName.Login:
          Login();
          break;
        case CommandName.Logout:
          Logout();
          break;
        case CommandName.Connect:
          Connect();
          break;
        case CommandName.Disconnect:
          Disconnect();
          break;
        case CommandName.Help:
          DisplayHelp();
          break;
        case CommandName.Exit:
          Exit();
          break;
      }
    }

    private void ExecuteExternal()
    {
      Console.WriteLine(_connection.Send(_command));
    }

    #endregion "EXECUTE COMMAND"

    #region "INTERNAL COMMANDS"

    private void Login()
    {
      Console.Write("Please, enter your name: ");
      string username = Console.ReadLine();
      if (string.IsNullOrWhiteSpace(username) || username.Contains(" "))
      {
        Console.WriteLine("Username is empty or invalid.");
        return;
      }
      Console.WriteLine(_connection.Login(username.ToLower(), "pwd"));
      _connection.Reconnect();
    }

    private void Logout()
    {
      Disconnect();
      Console.WriteLine(_connection.Logout());
    }

    private void Connect()
    {
      Console.WriteLine(_connection.Connect());
    }

    private void Disconnect()
    {
      Console.WriteLine(_connection.Disconnect());
    }

    private void DisplayHelp()
    {
      IList<CommandName> commandNames = _commands.GetCommandNames();
      foreach (CommandName commandName in commandNames)
        Console.WriteLine(commandName);
    }

    private void Exit()
    {
      Logout();
      _inProcess = false;
    }

    #endregion "INTERNAL COMMANDS"
  }
}
