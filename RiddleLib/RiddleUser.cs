using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RiddleLib
{
	public class RiddleUser
	{
		public Socket Socket;
		public Riddle Riddle;

		public RiddleUser(Socket socket) {
			Socket = socket;
			Riddle = RiddleApi.RequestNewRiddle().Result;
		}
	}
}
