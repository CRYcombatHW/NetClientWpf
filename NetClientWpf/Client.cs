using NetworkService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetClientWpf
{
	public delegate void MessageDelegate(NetworkMessage message);
	public delegate void DisconnectionDelegate();

	public class Client
	{
		public bool IsConnected {
			get {
				return _socket.Connected;
			}
		}

		private Socket _socket;

		public event MessageDelegate? OnMessage;
		public event DisconnectionDelegate? OnDisconnect;

		public Client() {
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public async Task<bool> TryConnectAsync(IPEndPoint endPoint) {
			try {
				await _socket.ConnectAsync(endPoint);
				return true;
			}
			catch (Exception x) {
				Console.WriteLine(x);
				return false;
			}
		}

		public async Task ListenMessages() {
			//todo interface

			byte[] buffer = new byte[1024 * 24];
			int size;

			MemoryStream ms;
			BinaryReader br;
			while (true) {
				try {
					Console.WriteLine("waitiong for message");
					size = await _socket.ReceiveAsync(buffer);
					Console.WriteLine("reciekele message");
				}
				catch {
					Console.WriteLine("Disconnected");
					OnDisconnect?.Invoke();
					return;
				}

				ms = new MemoryStream(buffer, 0, size);
				br = new BinaryReader(ms);
				Console.WriteLine($"ms legth: {ms.Length}");

				foreach (NetworkMessage message in br.ReadNetworkMessages()) {
					Console.WriteLine($"[Handling message]\n{message.Header}: {message.Content}");
					OnMessage?.Invoke(message);
					Console.WriteLine($"[Handling message]\n{message.Header}: {message.Content}");
				}
			}
		}

		public async Task SendMessage(NetworkMessage message) {
			//todo interface

			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);

			bw.Write(message);

			await _socket.SendAsync(ms.ToArray());
		}
	}
}
