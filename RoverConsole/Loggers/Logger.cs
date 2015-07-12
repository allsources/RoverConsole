using System;
using System.IO;

namespace RoverConsole.Loggers
{
  public static class Logger
  {
    #region "Private members"

    private static readonly string _logFileName;

    #endregion "Private members"

    #region ".ctor"

    static Logger()
    {
      _logFileName = Path.Combine(Directory.GetCurrentDirectory(), "log.txt");
    }

    #endregion ".ctor"

    #region "Public methods"

    public static void LogInfo(string msg)
    {
      LogMsg(GetMsgToLog("INFO", msg));
    }

    public static void LogException(Exception ex)
    {
      LogMsg(GetMsgToLog("ERROR", string.Format("{0} {1}", ex.Message, ex.StackTrace)));
    }

    #endregion "Public methods"

    #region "Private methods"

    private static string GetMsgToLog(string msgType, string msg)
    {
      return
        string.Format("{0:yyyy-MM-dd hh:mm:ss}\t{1}\t{2}", DateTime.Now, msgType, msg);
    }

    private static void LogMsg(string msgToLog)
    {
      using (TextWriter tw = TextWriter.Synchronized(File.AppendText(_logFileName)))
      {
        tw.WriteLine(msgToLog);
        tw.Close();
      }
    }

    #endregion "Private methods"
  }
}
