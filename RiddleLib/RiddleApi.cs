using NetworkService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RiddleLib
{
	public static class RiddleApi
	{
		private static HttpClient _httpClient = new HttpClient();

		public static async Task<Riddle> RequestNewRiddle() {
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ConfigurationManager.RiddleApiUrl);
			request.Headers.Add("X-Api-Key", ConfigurationManager.RiddleApiKey);

			HttpResponseMessage responce = await _httpClient.SendAsync(request);
			string jsonString = await responce.Content.ReadAsStringAsync();

			RiddleResponse riddleResponse = (
				JsonSerializer.Deserialize<List<RiddleResponse>>(jsonString)
				?? throw new Exception("Riddle api problemo")
			).First();

			Console.WriteLine($"Created new riddle: {{\n\tRiddle: {riddleResponse.question}\n\tAnswer: \"{riddleResponse.answer}\"\n}}");

			return new Riddle(riddleResponse.question, riddleResponse.answer);
		}
	}
}
