using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService
{
	public delegate void ConnectionDelegate(Socket sender);
	public delegate void DisconnectionDelegate(Socket sender);

	public delegate void MessageDelegate(Socket sender, NetworkMessage message);

	public delegate void BeforeMessageDelegate();
	public delegate void AfterMessageDelegate();
}
