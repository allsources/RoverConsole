using RoverConsole.Enums;

namespace RoverConsole.Classes
{
  public class ConsoleCommandConfig
  {
    #region "PUBLIC MEMBERS"

    public CommandName Name { get; set; }

    public CommandCall Call { get; set; }

    public string[] ArgumentValidators { get; set; }

    #endregion "PUBLIC MEMBERS"

    #region ".ctor"

    public ConsoleCommandConfig()
    {
      Name = CommandName.Unknown;
      Call = CommandCall.Internal;
      ArgumentValidators = new string[] { };
    }

    public ConsoleCommandConfig(CommandName commandName) : this()
    {
      Name = commandName;
    }

    public ConsoleCommandConfig(CommandName commandName, CommandCall call, params string[] argValidators)
    {
      Name = commandName;
      Call = call;
      ArgumentValidators = argValidators ?? new string[] { };
    }

    #endregion ".ctor"
  }
}
