
namespace RoverConsole.Classes
{
  public class ConsoleUser
  {
    #region "PRIVATE MEMBERS"

    public string Name { get; private set; }

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public ConsoleUser(string name)
    {
      Name = name;
    }

    #endregion ".ctor"
  }
}
