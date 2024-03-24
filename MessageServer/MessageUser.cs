using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MessageServer
{
	public class MessageUser
	{
		public Socket Socket;
		public string Name;

		public MessageUser(Socket socket, string name) {
			Socket = socket;
			Name = name;
		}
	}
}
