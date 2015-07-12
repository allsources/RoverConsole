using System;

namespace RoverConsole.Classes
{
  public class ConsoleLogMessage
  {
    #region "PRIVATE MEMBERS"

    private DateTime _dateTime;

    #endregion "PRIVATE MEMBERS"

    #region "PUBLIC MEMBERS"

    public string TimeStamp { get { return _dateTime.ToString("yyyy-MM-dd hh:mm:ss"); } }

    public string Username { get; set; }

    public string Text { get; set; }

    #endregion "PUBLIC MEMBERS"

    #region ".ctor"

    public ConsoleLogMessage(string username, string msg)
    {
      _dateTime = DateTime.Now;
      Username = username;
      Text = msg;
    }

    #endregion ".ctor"
  }
}
