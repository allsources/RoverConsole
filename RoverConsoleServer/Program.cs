using Microsoft.Owin.Hosting;
using RoverConsole.Classes;
using RoverConsole.Constants;
using System;
using System.Configuration;

namespace RoverConsole
{
  class Program
  {
    #region "PRIVATE MEMBERS"

    private static string _host;

    private static int _port;

    #endregion "PRIVATE MEMBERS"

    #region "MAIN"

    static void Main(string[] args)
    {
      if (InitializeServer())
        ServerStart();
    }

    #endregion "MAIN"

    #region "SERVER"

    private static bool InitializeServer()
    {
      _host = ConfigurationManager.AppSettings[ConsoleConstants.Host].ToString();
      _port = Convert.ToInt32(ConfigurationManager.AppSettings[ConsoleConstants.Port]);

      return
        !string.IsNullOrWhiteSpace(_host) && _port > 0;
    }

    private static void ServerStart()
    {
      string url = string.Format("http://{0}:{1}", _host, _port);
      using (WebApp.Start(url))
      {
        Console.WriteLine("Server running on {0}", url);
        Console.WriteLine("Press Enter to terminate.");
        Console.ReadLine();
      }
    }

    #endregion "SERVER"
  }
}
