using NetworkService;
using System.Net.Sockets;

namespace Server
{
	internal class Program
	{
		static async Task Main(string[] args) {
			Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			server.Bind(Service.RiddleServerEndpoint);
		}
	}
}