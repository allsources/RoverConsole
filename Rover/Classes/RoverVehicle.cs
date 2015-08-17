using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rover.Classes
{
  public class RoverVehicle
  {
    #region "PRIVATE MEMBERS"

    private readonly RoverConfig _config;

    private readonly RoverCommunicator _communicator;

    private readonly RoverNavigator _navigator;

    #endregion "PRIVATE MEMBERS"

    #region ".ctor"

    public RoverVehicle(RoverConfig config, RoverCommunicator communicator, RoverNavigator navigator)
    {
      _config = config;
      _communicator = communicator;
      _navigator = navigator;
    }

    #endregion ".ctor"

    #region "PUBLIC METHODS"

    public bool Start()
    {
      Console.WriteLine(_communicator.Login("1234567890"));
      _communicator.Reconnect();

      return true;
    }

    #endregion "PUBLIC METHODS"
  }
}
