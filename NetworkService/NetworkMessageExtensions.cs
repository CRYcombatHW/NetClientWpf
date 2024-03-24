namespace NetworkService
{
	public static class NetworkMessageExtensions
	{
		public static NetworkMessage ReadNetworkMessage(this BinaryReader br) {
			return new NetworkMessage(
				br.ReadString(),
				br.ReadString()
			);
		}
		public static List<NetworkMessage> ReadNetworkMessages(this BinaryReader br) {
			List<NetworkMessage> messages = new List<NetworkMessage>();

			while (br.BaseStream.Position < br.BaseStream.Length) {
				messages.Add(ReadNetworkMessage(br));
			}

			return messages;
		}

		public static void Write(this BinaryWriter bw, NetworkMessage message) {
			bw.Write(message.Header);
			bw.Write(message.Content);
		}
	}
}
