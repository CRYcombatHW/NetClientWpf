namespace NetworkService
{
	public class NetworkMessage
	{
		public string Header;
		public string Content;

		public NetworkMessage(string header, string content) {
			Header = header;
			Content = content;
		}
	}
}
