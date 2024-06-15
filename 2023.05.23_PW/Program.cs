using System.Net.Http.Headers;

namespace _2023._05._23_PW
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Please, Enter the uri:");
			string uri = Console.ReadLine()!;

			InvokeRequestResponseService(uri).Wait();
		}

		static async Task InvokeRequestResponseService(string uri)
		{
			HttpClient client = new();

			using HttpRequestMessage httpRequest = new(HttpMethod.Post, uri);

			var requestBody = @"{
                  ""Inputs"": {
                    ""data"": [
                      {
                        ""instant"": 732,
                        ""date"": ""2013-01-01T00:00:00.000Z"",
                        ""season"": 1,
                        ""yr"": 0,
                        ""mnth"": 1,
                        ""weekday"": 6,
                        ""weathersit"": 2,
                        ""temp"": 0.344167,
                        ""atemp"": 0.363635,
                        ""hum"": 0.805833,
                        ""windspeed"": 0.160446
                      }
                    ]
                  }
                }";

			httpRequest.Content = new StringContent(requestBody);
			httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

			HttpResponseMessage response = await client.SendAsync(httpRequest);

			if (response.IsSuccessStatusCode)
			{
				string result = await response.Content.ReadAsStringAsync();
				Console.WriteLine("Result: {0}", result);
			}
			else
			{
				Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));
				Console.WriteLine(response.Headers.ToString());
				string responseContent = await response.Content.ReadAsStringAsync();
				Console.WriteLine(responseContent);
			}
		}
	}
}
