using NetworkService;
using RiddleLib;
using ServerLib;
using System.Net.Sockets;

namespace RiddleServer
{
	internal class Program
	{
		private static NetworkServer _server = new NetworkServer(ConfigurationManager.RiddleServerEndpoint);

		private static async Task Main(string[] args) {
			Console.WriteLine("Staring riddle server");

			List<RiddleUser> users = new List<RiddleUser>();

			_server.OnConnection += async (socket) => {
				RiddleUser user = new RiddleUser(socket);

				users.Add(user);
				await _server.SendMessage(user.Socket, new NetworkMessage("Server", $"Riddle:\n{user.Riddle.Content}"));
			};
			_server.OnMessage += async (socket, message) => {
				foreach (RiddleUser user in users) {
					if (user.Socket.RemoteEndPoint == socket.RemoteEndPoint) {
						await HandleMessage(user, message);
					}
				}
			};

			await _server.AcceptClientsAsync();
		}

		private static async Task HandleMessage(RiddleUser user, NetworkMessage message) {
			Console.WriteLine($"[Recieved message from {user.Socket.RemoteEndPoint}]\n{message.Header}: {message.Content}");
			message.Content = message.Content.ToLower();

			if (user.Riddle.IsCorrectAnswer(message.Content)) {
				await _server.SendMessage(user.Socket, new NetworkMessage("Server", "Correct answer!"));
			}
			else if (message.Content == "!giveup") {
				await _server.SendMessage(user.Socket, new NetworkMessage("Server", $"Answer:\n{user.Riddle.Answer}"));
			}
			else {
				await _server.SendMessage(user.Socket, new NetworkMessage("Server", "Wrong!"));
				return;
			}

			await user.Riddle.UpdateRiddle();
			await _server.SendMessage(user.Socket, new NetworkMessage("Server", $"Riddle:\n{user.Riddle.Content}"));
		}
	}
}
