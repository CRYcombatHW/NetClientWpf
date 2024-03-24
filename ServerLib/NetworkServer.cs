using System.Net.Sockets;
using System.Net;
using NetworkService;

namespace ServerLib
{
	public class NetworkServer : NetworkSocket
	{
		public NetworkServer(IPEndPoint endPoint) {
			_socket.Bind(endPoint);
			_socket.Listen();
		}

		public async Task AcceptClientsAsync() {
			Console.WriteLine("Waiting for clients");

			Socket client;
			while (true) {
				client = await _socket.AcceptAsync();
				Console.WriteLine($"Client {client.RemoteEndPoint} acceped");

				Task.Run(() => ListenSocket(client));
			}
		}
	}
}