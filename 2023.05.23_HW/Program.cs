﻿using System.Net.Http.Headers;

namespace _2023._05._23_PW
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Please, Enter the uri:");
			string uri = Console.ReadLine()!;
			Console.WriteLine("Please, Enter the path to the JSON file:");
			string content = File.ReadAllText(Console.ReadLine()!);
			Console.WriteLine();

			InvokeRequestResponseService(uri, content).Wait();
		}

		static async Task InvokeRequestResponseService(string uri, string jsonContent)
		{
			HttpClient client = new();

			using HttpRequestMessage httpRequest = new(HttpMethod.Post, uri);

			httpRequest.Content = new StringContent(jsonContent);
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
