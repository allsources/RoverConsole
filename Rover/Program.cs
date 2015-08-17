using Rover.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover
{
  class Program
  {
    #region "PRIVATE MEMBERS"

    static RoverVehicle _rover;

    #endregion "PRIVATE MEMBERS"

    #region "MAIN"

    static void Main(string[] args)
    {
      if (InitializeRover())
        ProcessRover();
    }

    #endregion "MAIN"

    #region "ROVER INITIALIZATION"

    static bool InitializeRover()
    {
      RoverConfig config = InitializeConfig();
      if (config == null)
        return false;

      RoverCommunicator communicator = InitializeCommunicator();
      if (communicator == null)
        return false;

      RoverNavigator navigator = InitializeNavigator();
      if (navigator == null)
        return false;

      Console.Write("Initialize rover... ");
      _rover = new RoverVehicle(config, communicator, navigator);
      Console.WriteLine(_rover != null ? "DONE" : "FAIL");

      return _rover != null;
    }

    static RoverConfig InitializeConfig()
    {
      Console.Write("Initialize config... ");

      var config = new RoverConfig();

      Console.WriteLine(config != null ? "DONE" : "FAIL");

      return config;
    }

    static RoverCommunicator InitializeCommunicator()
    {
      Console.Write("Initialize communicator... ");

      string hubUrl = ConfigurationManager.AppSettings["HubURL"];
      string hubName = ConfigurationManager.AppSettings["HubName"];

      var communicator = new RoverCommunicator(hubUrl, hubName);

      Console.WriteLine(communicator != null ? "DONE" : "FAIL");

      return communicator;
    }

    static RoverNavigator InitializeNavigator()
    {
      Console.Write("Initialize navigator... ");

      var navigator = new RoverNavigator();

      Console.WriteLine(navigator != null ? "DONE" : "FAIL");

      return navigator;
    }

    #endregion "ROVER INITIALIZATION"

    #region "ROVER PROCESS"

    static void ProcessRover()
    {
      _rover.Start();
      Console.Read();
    }

    #endregion "ROVER PROCESS"
  }
}
