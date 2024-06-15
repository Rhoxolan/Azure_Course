using System.Net.Http.Headers;

namespace _2023._05._23_PW
{
	internal class Program
	{
		private const string uri = "http://6885b123-ca19-4e77-8110-5f976a7248ae.polandcentral.azurecontainer.io/score";

		static void Main(string[] args)
		{
			InvokeRequestResponseService().Wait();
		}

		static async Task InvokeRequestResponseService()
		{
			HttpClient client = new();

			using HttpRequestMessage httpRequest = new(HttpMethod.Post, uri);

			var requestBody = @"{
                  ""Inputs"": {
                    ""data"": [
                      {
                        ""ID"": ""example_value"",
                        ""name"": ""example_value"",
                        ""category"": ""example_value"",
                        ""main_category"": ""example_value"",
                        ""currency"": ""example_value"",
                        ""deadline"": ""example_value"",
                        ""goal"": ""example_value"",
                        ""launched"": ""example_value"",
                        ""pledged"": ""example_value"",
                        ""backers"": ""example_value"",
                        ""country"": ""example_value"",
                        ""usd pledged"": 0.0,
                        ""usd_pledged_real"": ""example_value"",
                        ""usd_goal_real;;;;"": ""example_value"",
                        ""Column16"": ""example_value"",
                        ""Column17"": ""example_value"",
                        ""Column18"": ""example_value"",
                        ""Column19"": ""example_value""
                      }
                    ]
                  },
                  ""GlobalParameters"": {
                    ""method"": ""predict""
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
