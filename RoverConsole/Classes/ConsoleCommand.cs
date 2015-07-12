using RoverConsole.Enums;

namespace RoverConsole.Classes
{
  public class ConsoleCommand : ConsoleCommandConfig
  {
    #region "PUBLIC MEMBERS"

    public string[] Arguments { get; set; }

    public CommandValidationStatus ValidationStatus { get; set; }

    #endregion "PUBLIC MEMBERS"

    #region ".ctor"

    public ConsoleCommand() : base()
    {
      Arguments = new string[] { };
      ValidationStatus = CommandValidationStatus.Unvalidated;
    }

    public ConsoleCommand(CommandName name) : base(name)
    {
    }

    public ConsoleCommand(ConsoleCommandConfig config, CommandValidationStatus validationStatus, params string[] arguments)
      : base(config.Name, config.Call, config.ArgumentValidators)
    {
      Arguments = arguments ?? new string[] { };
      ValidationStatus = validationStatus;
    }

    #endregion ".ctor"
  }
}
