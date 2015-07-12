using RoverConsole.Classes;
using RoverConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoverConsoleClient.Classes
{
  public class ConsoleCommands
  {
    #region "PRIVATE MEMBERS"

    private IList<ConsoleCommandConfig> _commandConfigs;

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public ConsoleCommands(IList<ConsoleCommandConfig> commandConfigs)
    {
      _commandConfigs = commandConfigs ?? new List<ConsoleCommandConfig>();
    }

    #endregion ".ctor"

    #region "COMMAND VALIDATION"

    public CommandValidationStatus Validate(string[] commandRawData)
    {
      CommandValidationStatus validationStatus;
      return
        HasData(commandRawData, out validationStatus) &&
        CommandExists(commandRawData, out validationStatus) &&
        ValidNumberOfArguments(commandRawData, out validationStatus) &&
        ValidArgumentTypes(commandRawData, out validationStatus)
          ? CommandValidationStatus.Ok
          : validationStatus;
    }

    private bool HasData(string[] commandRawData, out CommandValidationStatus validationStatus)
    {
      validationStatus =
        commandRawData != null &&
        commandRawData.Any()
          ? CommandValidationStatus.Ok
          : CommandValidationStatus.Unvalidated;

      return
        validationStatus == CommandValidationStatus.Ok;
    }

    private bool CommandExists(string[] commandRawData, out CommandValidationStatus validationStatus)
    {
      CommandName commandName;
      Enum.TryParse(commandRawData[0], true, out commandName);

      validationStatus =
        commandName != CommandName.Unknown
          ? CommandValidationStatus.Ok
          : CommandValidationStatus.CommandNotFound;

      return
        validationStatus == CommandValidationStatus.Ok;
    }

    private bool ValidNumberOfArguments(string[] commandRawData, out CommandValidationStatus validationStatus)
    {
      ConsoleCommandConfig commandConfig = GetCommandConfig(commandRawData);

      validationStatus =
        commandConfig.ArgumentValidators.Length == commandRawData.Length - 1
          ? CommandValidationStatus.Ok
          : CommandValidationStatus.InvalidNumberOfArguments;

      return
        validationStatus == CommandValidationStatus.Ok;
    }

    private bool ValidArgumentTypes(string[] commandRawData, out CommandValidationStatus validationStatus)
    {
      ConsoleCommandConfig commandConfig = GetCommandConfig(commandRawData);

      validationStatus = CommandValidationStatus.Ok;
      for (int i = 0; i < commandConfig.ArgumentValidators.Length; i++)
      {
        var re = new Regex(commandConfig.ArgumentValidators[i], RegexOptions.IgnoreCase);
        if (re.IsMatch(commandRawData[i+1]))
          continue;
        validationStatus = CommandValidationStatus.ArgumentHasInvalidValue;
        break;
      }

      return
        validationStatus == CommandValidationStatus.Ok;
    }

    #endregion "COMMAND VALIDATION"

    #region "PARSE COMMAND"

    public ConsoleCommand Parse(string commandRawData)
    {
      string[] arr =
        commandRawData != null
          ? commandRawData.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)
          : new string[] { };
      return
        Parse(arr);
    }

    public ConsoleCommand Parse(string[] commandRawData)
    {
      CommandName commandName = GetCommandName(commandRawData);
      ConsoleCommandConfig config = GetCommandConfig(commandName);
      string[] args = GetCommandArguments(commandRawData);
      CommandValidationStatus validationStatus = Validate(commandRawData);
      return
        new ConsoleCommand(config, validationStatus, args);
    }

    #endregion "PARSE COMMAND"

    #region "COMMAND DESCRIPTION"

    public IList<CommandName> GetCommandNames()
    {
      return
        _commandConfigs
          .Select(cmd => cmd.Name).ToList();
    }

    #endregion "COMMAND DESCRIPTION"

    #region "PRIVATE HELPER METHODS"

    private CommandName GetCommandName(string[] commandRawData)
    {
      string commandName =
        commandRawData != null && commandRawData.Any()
          ? commandRawData[0]
          : string.Empty;
      CommandName cn;
      Enum.TryParse(commandName, true, out cn);
      return cn;
    }

    private ConsoleCommandConfig GetCommandConfig(string[] commandRawData)
    {
      return
        GetCommandConfig(GetCommandName(commandRawData));
    }

    private ConsoleCommandConfig GetCommandConfig(CommandName commandName)
    {
      return
        _commandConfigs.FirstOrDefault(cmd => cmd.Name == commandName) ??
        new ConsoleCommandConfig();
    }

    private string[] GetCommandArguments(string[] commandRawData)
    {
      return
        commandRawData != null
          ? commandRawData.Skip(1).ToArray()
          : new string[] { };
    }

    #endregion "PRIVATE HELPER METHODS"
  }
}
