using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoverConsole.Enums
{
  public enum ConsoleConnectionStatus
  {
    Undefined,

    Ok,

    OperationException,

    NoConnection,

    Connected,

    AlreadyConnected,

    Disconnected,

    AlreadyDisconnected,

    Reconnected,

    Authenticated,

    AlreadyAuthenticated,

    Logout,

    AlreadyLogout,

    IncorrectUsername,
  }
}
