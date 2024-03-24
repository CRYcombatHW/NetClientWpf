using NetworkService;
using System.Net;
using System.Net.Sockets;

namespace ClientLib
{
	public class NetworkClient : NetworkSocket
	{
		public async Task<bool> TryConnectAsync(IPEndPoint endPoint) {
			try {
				await _socket.ConnectAsync(endPoint);
				return _socket.Connected;
			}
			catch (SocketException) {
				return false;
			}
		}

		public async Task Listen()
			=> await ListenSocket(_socket);

		public async Task SendMessage(NetworkMessage message)
			=> await SendMessage(_socket, message);
	}
}