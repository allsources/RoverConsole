using Microsoft.VisualStudio.TestTools.UnitTesting;
using RoverConsole.Classes;
using RoverConsole.Enums;
using RoverConsoleClient.Classes;
using System.Collections.Generic;

namespace RoverConsoleTests
{
  [TestClass]
  public class ConsoleCommandTests
  {
    #region "PRIVATE MEMBERS"

    private IList<ConsoleCommandConfig> _commandConfigs =
      new List<ConsoleCommandConfig>
      {
        new ConsoleCommandConfig(CommandName.Unknown),
        new ConsoleCommandConfig(CommandName.Exit),
        new ConsoleCommandConfig(CommandName.Scan, CommandCall.External, null),
        new ConsoleCommandConfig(CommandName.Move, CommandCall.External, @"\b(top|right|bottom|left)\b", @"\b[1-5]\b"),
        new ConsoleCommandConfig(CommandName.Restart, CommandCall.External, null),
      };

    #endregion "PRIVATE MEMBERS"

    #region "CONSOLE COMMAND VALIDATION"

    [TestMethod]
    public void ValidateNullCommand()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = null;
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.Unvalidated);
    }

    [TestMethod]
    public void ValidateEmptyCommand()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = {};
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.Unvalidated);
    }

    [TestMethod]
    public void ValidateUnknownCommand()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "unknownCommand", "12345" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.CommandNotFound);
    }

    [TestMethod]
    public void ValidateCommandWithMissedArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.InvalidNumberOfArguments);
    }

    [TestMethod]
    public void ValidateCommandWithFewerArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "top" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.InvalidNumberOfArguments);
    }

    [TestMethod]
    public void ValidateCommandWithMoreArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "top", "12345", "more" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.InvalidNumberOfArguments);
    }

    [TestMethod]
    public void ValidateCommandWithInvalidTextArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "qwerty", "12345" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.ArgumentHasInvalidValue);
    }

    [TestMethod]
    public void ValidateCommandWithInvalidNumericArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "left", "qwerty" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.ArgumentHasInvalidValue);
    }

    [TestMethod]
    public void ValidateCommandWithOutOfRangeNumericArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "right", "123" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.ArgumentHasInvalidValue);
    }

    [TestMethod]
    public void ValidateCommandWithoutArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "exit" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.Ok);
    }

    [TestMethod]
    public void ValidateCommandWithArguments()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "top", "2" };
      CommandValidationStatus validationStatus = commands.Validate(commandRawData);
      Assert.IsTrue(validationStatus == CommandValidationStatus.Ok);
    }

    #endregion "CONSOLE COMMAND VALIDATION"

    #region "PARSE CONSOLE COMMAND"

    [TestMethod]
    public void ParseEmptyCommand()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { };
      ConsoleCommand command = commands.Parse(commandRawData);
      Assert.IsTrue(
        command.Name == CommandName.Unknown &&
        command.ValidationStatus == CommandValidationStatus.Unvalidated);
    }

    [TestMethod]
    public void ParseUnknownCommand()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "qwerty", null };
      ConsoleCommand command = commands.Parse(commandRawData);
      Assert.IsTrue(
        command.Name == CommandName.Unknown &&
        command.ValidationStatus == CommandValidationStatus.CommandNotFound);
    }

    [TestMethod]
    public void ParseValidCommand()
    {
      ConsoleCommands commands = new ConsoleCommands(_commandConfigs);
      string[] commandRawData = { "move", "top", "4" };
      ConsoleCommand command = commands.Parse(commandRawData);
      Assert.IsTrue(
        command.Name == CommandName.Move &&
        command.Call == CommandCall.External &&
        command.ValidationStatus == CommandValidationStatus.Ok &&
        command.Arguments.Length == 2 && command.Arguments[0] == "top" && command.Arguments[1] == "4");
    }

    #endregion "PARSE CONSOLE COMMAND"
  }
}
