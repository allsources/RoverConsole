using RoverConsole.Classes;
using RoverConsole.Enums;
using System.Collections.Generic;

namespace RoverConsoleClient.Helpers
{
  public static class ResourceHelper
  {
    #region "PUBLIC METHODS"

    public static IList<ConsoleCommandConfig> LoadConsoleCommandConfigs()
    {
      return
        new List<ConsoleCommandConfig>
        {
          new ConsoleCommandConfig(CommandName.Login),
          new ConsoleCommandConfig(CommandName.Logout),
          new ConsoleCommandConfig(CommandName.Connect),
          new ConsoleCommandConfig(CommandName.Disconnect),
          new ConsoleCommandConfig(CommandName.Scan, CommandCall.External, null),
          new ConsoleCommandConfig(CommandName.Move, CommandCall.External, @"\b(top|right|bottom|left)\b", @"\b[1-5]\b"),
          new ConsoleCommandConfig(CommandName.Restart, CommandCall.External, null),
          new ConsoleCommandConfig(CommandName.Help),
          new ConsoleCommandConfig(CommandName.Exit),
        };
    }

    #endregion "PUBLIC METHODS"
  }
}
