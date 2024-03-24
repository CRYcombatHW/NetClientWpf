using Microsoft.Extensions.Configuration;
using System.Net;

namespace NetworkService
{
	public static class ConfigurationManager
	{
		public static Uri RiddleApiUrl { get; }
		public static string RiddleApiKey { get; }

		public static IPEndPoint RiddleServerEndpoint { get; }
		public static IPEndPoint MessageServerEndpoint { get; }

		static ConfigurationManager() {
			IConfigurationBuilder builder = new ConfigurationBuilder()
							  .SetBasePath(AppContext.BaseDirectory)
							  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			IConfigurationRoot configuration = builder.Build();

			string RiddleServerIp = configuration.GetRequiredSection("IPAddresses:RiddleServerIP").Value ?? "127.0.0.1";
			string MessageServerIp = configuration.GetRequiredSection("IPAddresses:MessageServerIp").Value ?? "127.0.0.1";

			int RiddleServerPort = int.Parse(configuration.GetRequiredSection("Ports:RiddleServerPort").Value ?? "50631");
			int MessageServerPort = int.Parse(configuration.GetRequiredSection("Ports:MessageServerPort").Value ?? "50632");

			RiddleApiUrl = new Uri(configuration.GetRequiredSection("API:Ninjas:Url").Value ?? "https://api.api-ninjas.com/v1/riddles");
			RiddleApiKey = configuration.GetRequiredSection("API:Ninjas:Key").Value ?? throw new Exception("cant find required NinjasAPI key");

			RiddleServerEndpoint = new IPEndPoint(
				IPAddress.Parse(RiddleServerIp),
				RiddleServerPort
			);
			MessageServerEndpoint = new IPEndPoint(
				IPAddress.Parse(MessageServerIp),
				MessageServerPort
			);
		}
	}
}
