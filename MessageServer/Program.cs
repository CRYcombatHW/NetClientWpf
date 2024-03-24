using ServerLib;
using System.Net;
using System.Net.Sockets;
using NetworkService;

namespace MessageServer
{
	internal class Program
	{
		private static NetworkServer _server = new NetworkServer(ConfigurationManager.MessageServerEndpoint);

		private static async Task Main(string[] args) {
			Console.WriteLine("Staring message server");

			List<MessageUser> users = new List<MessageUser>();

			_server.OnConnection += async (socket) => {
				MessageUser newUser = new MessageUser(socket, $"User {users.Count + 1}");
				users.Add(newUser);

				foreach (MessageUser user in users) {
					await _server.SendMessage(user.Socket, new NetworkMessage("Service", $"New user \"{newUser.Name}\" connected"));
				}
			};
			_server.OnMessage += async (socket, message) => {
				foreach (MessageUser user in users) {
					if (user.Socket.RemoteEndPoint != socket.RemoteEndPoint) {
						message.Header = user.Name;

						await _server.SendMessage(user.Socket, message);
					}
				}
			};
			_server.OnDisconnection += async (socket) => {
				MessageUser? disconnectedUser = null;

				foreach (MessageUser user in users) {
					if (user.Socket.RemoteEndPoint == socket.RemoteEndPoint) {
						disconnectedUser = user;
						break;
					}
				}
				if (disconnectedUser is null) {
					return;
				}

				foreach (MessageUser user in users) {
					if (disconnectedUser.Socket.RemoteEndPoint != user.Socket.RemoteEndPoint) {
						await _server.SendMessage(user.Socket, new NetworkMessage("Service", $"New user \"{disconnectedUser.Name}\" disconnected"));
					}
				}
			};

			await _server.AcceptClientsAsync();
		}
	}
}
