using _2023._05._19_PW.Models;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2023._05._19_PW.Services.HrefProcessingService
{
	public class HrefProcessingService : IHrefProcessingService
	{
		public async Task AddRowKeyToQueueAsync(IAsyncCollector<string> queue, string rowKey)
		{
			await queue.AddAsync(rowKey);
		}

		public async Task<string?> GetHrefFromHttpRequestAsync(HttpRequest req)
		{
			string? href = req.Query["href"];
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic? data = JsonConvert.DeserializeObject(requestBody);
			href ??= data?.href;
			return href;
		}

		public async Task<(string RowKey, string Url)?> GetShortUrlAsync(string shortUrl, TableClient tableClient)
		{
			shortUrl = shortUrl.ToUpper();
			var result = await tableClient.GetEntityIfExistsAsync<UrlData>(partitionKey: shortUrl[0].ToString(), shortUrl);
			if (!result.HasValue)
			{
				return null;
			}
			return (result.Value.RowKey, result.Value.Url);
		}

		public async Task<string> RegisterShortUrlAsync(TableClient tableClient, string href)
		{
			UrlKey urlKey;
			var result = await tableClient.GetEntityIfExistsAsync<UrlKey>("1", "Key");
			if (!result.HasValue)
			{
				urlKey = new UrlKey() { Id = 1024, PartitionKey = "1", RowKey = "Key" };
				await tableClient.UpsertEntityAsync(urlKey);
			}
			else
			{
				urlKey = result.Value;
			}
			int index = urlKey.Id;
			string code = "";
			string alphabet = "ABCDEFGHIGKLMNOPQRSTUVWXYZ";
			while (index > 0)
			{
				code += alphabet[index % alphabet.Length];
				index /= alphabet.Length;
			}
			code = string.Join(string.Empty, code.Reverse());
			UrlData urlData = new UrlData
			{
				RowKey = code,
				PartitionKey = code[0].ToString(),
				Url = href,
				Count = 1,
				Id = code
			};
			urlKey.Id++;
			await tableClient.UpsertEntityAsync(urlData);
			await tableClient.UpsertEntityAsync(urlKey);
			return urlData.RowKey;
		}

		public async Task UpdateHrefObtainCount(string shortCode, TableClient tableClient)
		{
			var result = await tableClient.GetEntityIfExistsAsync<UrlData>(
				partitionKey: shortCode[0].ToString(),
				shortCode);
			result.Value.Count++;
			await tableClient.UpsertEntityAsync(result.Value);
		}
	}
}
