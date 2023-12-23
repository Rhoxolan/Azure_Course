using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Data.Tables;
using _2023._05._19_PW.Models;
using System.Linq;

namespace _2023._05._19_PW
{
    public class FunctionUrlShortener
    {
        [FunctionName("Set")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
			[Table("shortUrl")] TableClient tableClient,
			ILogger log)
        {
            string href = req.Query["href"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            href ??= data?.href;

			if (string.IsNullOrEmpty(href))
            {
				return new OkObjectResult("Please send href parameter via body or query like " +
					"http://localhost:7015/api/set?href=https://mystat.itstep.org/index");
			}

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

			return new OkObjectResult(new { href, shortUrl = urlData.RowKey });
		}

		[FunctionName("Get")]
		public async Task<IActionResult> Get(
			[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "get/{shortUrl}")] HttpRequest req,
			string shortUrl,
			[Table("shortUrl")] TableClient tableClient,
			[Queue("counts")] IAsyncCollector<string> queue
			)
		{
			if (string.IsNullOrEmpty(shortUrl))
				return new BadRequestResult();

			shortUrl = shortUrl.ToUpper();
			var result = await tableClient.GetEntityIfExistsAsync<UrlData>(partitionKey: shortUrl[0].ToString(), shortUrl);

			if (!result.HasValue)
				return new BadRequestObjectResult("There's no such short URL!");

			await queue.AddAsync(result.Value.RowKey);

			return new OkObjectResult(result.Value.Url);
		}

		[FunctionName("ProcessQueue")]
		public async Task ProcessQueue(
			[QueueTrigger("counts")] string shortCode,
			[Table("shortUrl")] TableClient tableClient
			)
		{
			var result = await tableClient.GetEntityIfExistsAsync<UrlData>(
				partitionKey: shortCode[0].ToString(),
				shortCode);
			result.Value.Count++;
			await tableClient.UpsertEntityAsync(result.Value);
		}
	}
}
