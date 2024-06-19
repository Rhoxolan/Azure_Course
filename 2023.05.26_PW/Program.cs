using System.Net.Http.Headers;
using System.Text.Json;

namespace _2023._05._26_PW
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var dto = new AzureTranslateDTO();

			Console.Write("Please, enter the URI: ");
			dto.Uri = Console.ReadLine();

			Console.Write("Please, enter the key: ");
			dto.Key = Console.ReadLine();

			Console.Write("Please, enter the region value: ");
			dto.Region = Console.ReadLine();

			Console.Write("Please, enter the from value: ");
			dto.From = Console.ReadLine();

			Console.Write("Please, enter the to value: ");
			dto.To = Console.ReadLine();

			Console.Write("Please, enter the api version: ");
			dto.ApiVersion = Console.ReadLine();

			Console.Write("Please, enter the text to translate: ");
			dto.Text = Console.ReadLine();

			var result = await Translate(dto);

			Console.WriteLine(Environment.NewLine + result);
		}

		static async Task<string> Translate(AzureTranslateDTO dto)
		{
			HttpClient client = new();

			var uri = $"{dto.Uri}/translate?api-version={dto.ApiVersion}&from={dto.From}&to={dto.To}";
			var body = new object[] { new { dto.Text } };

			using HttpRequestMessage httpRequest = new(HttpMethod.Post, uri);

			httpRequest.Content = new StringContent(JsonSerializer.Serialize(body));
			httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			httpRequest.Headers.Add("Ocp-Apim-Subscription-Key", dto.Key);
			httpRequest.Headers.Add("Ocp-Apim-Subscription-Region", dto.Region);

			HttpResponseMessage response = await client.SendAsync(httpRequest);
			return await response.Content.ReadAsStringAsync();
		}
	}

	internal class AzureTranslateDTO
	{
		public string? Key { get; set; }

		public string? Region { get; set; }

		public string? Uri { get; set; }

		public string? From { get; set; }

		public string? To { get; set; }

		public string? ApiVersion { get; set; }

		public string? Text { get; set; }
	}
}
