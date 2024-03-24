using NetworkService;
using System.Net.Sockets;

namespace RiddleServer
{
	internal class Program
	{
		private static async Task Main(string[] args) {
			Console.WriteLine("Staring riddle server");

			Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			server.Bind(ConfigurationManager.RiddleServerEndpoint);
			server.Listen();

			await AcceptCientsAsync(server);
		}

		private static async Task AcceptCientsAsync(Socket server) {
			Console.WriteLine("Waiting for clients");

			Socket client;
			while (true) {
				client = await server.AcceptAsync();
				Console.WriteLine($"Client {client.RemoteEndPoint} acceped");

				Task.Run(() => WorkWithClientAsync(client));
			}
		}

		private static async Task WorkWithClientAsync(Socket client) {
			Riddle riddle = await RiddleApi.RequestNewRiddle();
			await SendMessage(client, new NetworkMessage("Server", $"Riddle:\n\"{riddle.Content}\"\n(type \"!giveup\" to give up)"));

			byte[] buffer = new byte[1024 * 24];
			int size;

			MemoryStream ms;
			BinaryReader br;
			while (true) {
				try {
					size = await client.ReceiveAsync(buffer);
				}
				catch {
					Console.WriteLine("Client disconnected");
					return;
				}

				ms = new MemoryStream(buffer, 0, size);
				br = new BinaryReader(ms);

				foreach (NetworkMessage message in br.ReadNetworkMessages()) {
					await HandleMessage(client, riddle, message);
				}
			}
		}

		private static async Task SendMessage(Socket targetSocket, NetworkMessage message) {
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);

			bw.Write(message);

			await targetSocket.SendAsync(ms.ToArray());
		}

		private static async Task HandleMessage(Socket senderClient, Riddle riddle, NetworkMessage message) {
			Console.WriteLine($"[Recieved message from {senderClient.RemoteEndPoint}]\n{message.Header}: {message.Content}");
			message.Content = message.Content.ToLower();

			if (riddle.IsCorrectAnswer(message.Content)) {
				await SendMessage(senderClient, new NetworkMessage("Server", "Correct answer!"));
			}
			else if (message.Content == "!giveup") {
				await SendMessage(senderClient, new NetworkMessage("Server", $"Answer:\n{riddle.Answer}"));
			}
			else {
				await SendMessage(senderClient, new NetworkMessage("Server", "Wrong!"));
				return;
			}

			await riddle.UpdateRiddle();
			await SendMessage(senderClient, new NetworkMessage("Server", $"Riddle:\n{riddle.Content}"));
		}
	}
}
