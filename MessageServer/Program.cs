using ServerLib;
using System.Net;
using System.Net.Sockets;
using NetworkService;
using System.Runtime.InteropServices;

namespace MessageServer
{
	internal class Program
	{
		private static List<NetworkMessage> _chatHistory = new List<NetworkMessage>();

		private static NetworkServer _server = new NetworkServer(ConfigurationManager.MessageServerEndpoint);

		private static async Task Main(string[] args) {
			Console.WriteLine("Staring message server");

			List<Socket> users = new List<Socket>();

			_server.OnConnection += async (socket) => {
                users.Add(socket);
                foreach (NetworkMessage message in _chatHistory)
				{
                    await _server.SendMessage(socket, message);
                }


                SaveChatHistory(new NetworkMessage("Service", $"User connected"));
                foreach (Socket user in users) {
					await _server.SendMessage(user, new NetworkMessage("Service", $"New user connected"));
				}
			};
			_server.OnMessage += async (socket, message) => {
                SaveChatHistory(message);
				foreach (Socket user in users) {
					if (user.RemoteEndPoint != socket.RemoteEndPoint) {
						await _server.SendMessage(user, message);
					}
				}
			};
			_server.OnDisconnection += async (socket) =>
			{
                Socket? disconnectedUser = null;

				disconnectedUser = users.FirstOrDefault(u => u.RemoteEndPoint == socket.RemoteEndPoint);

				if (disconnectedUser is null)
				{
					return;
				}
				users.Remove(disconnectedUser);
                SaveChatHistory(new NetworkMessage("Service", $"User disconnected"));
                foreach (Socket user in users)
				{
                    await _server.SendMessage(user, new NetworkMessage("Service", $"User disconnected"));
				}
			};

			await _server.AcceptClientsAsync();

		
		}
		private static void SaveChatHistory(NetworkMessage message)
		{
            _chatHistory.Add(message);
			Console.WriteLine($"[Saved to history] {message.Header}: {message.Content}");
        }
    }
}
