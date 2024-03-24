using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService
{
	public abstract class NetworkSocket
	{
		public event ConnectionDelegate? OnConnection;
		public event DisconnectionDelegate? OnDisconnection;
		public event MessageDelegate? OnMessage;
		public event BeforeMessageDelegate? BeforeMessage;
		public event AfterMessageDelegate? AfterMessage;

		protected Socket _socket;

		public NetworkSocket() {
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		}

		public async Task ListenSocket(Socket socket) {
			OnConnection?.Invoke(socket);

			byte[] buffer = new byte[1024 * 24];
			int size;

			MemoryStream ms;
			BinaryReader br;
			while (true) {
				try {
					size = await socket.ReceiveAsync(buffer);
				}
				catch {
					OnDisconnection?.Invoke(socket);
					return;
				}

				ms = new MemoryStream(buffer, 0, size);
				br = new BinaryReader(ms);

				foreach (NetworkMessage message in br.ReadNetworkMessages()) {
					OnMessage?.Invoke(socket, message);
				}
			}
		}

		public async Task SendMessage(Socket targetSocket, NetworkMessage message) {
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);

			bw.Write(message);

			await targetSocket.SendAsync(ms.ToArray());
		}
	}
}
