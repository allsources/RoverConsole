
namespace RoverConsole.Enums
{
  /// <summary>
  /// Represents a type of command call.
  /// </summary>
  public enum CommandCall
  {
    /// <summary>
    /// Executed place is not specified.
    /// </summary>
    Undefined,

    /// <summary>
    /// Command should be executed by the client console.
    /// </summary>
    Internal,

    /// <summary>
    /// Command should be sent to the server console.
    /// </summary>
    External,
  }
}
