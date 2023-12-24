using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Azure.Data.Tables;
using _2023._05._19_PW.Services.HrefProcessingService;

namespace _2023._05._19_PW
{
    public class FunctionUrlShortener
    {
		private readonly IHrefProcessingService _hrefProcessingService;

		public FunctionUrlShortener(IHrefProcessingService hrefProcessingService)
		{
			_hrefProcessingService = hrefProcessingService;
		}

		[FunctionName("Set")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
			[Table("shortUrl")] TableClient tableClient)
        {
			var href =  await _hrefProcessingService.GetHrefFromHttpRequestAsync(req);
			if (string.IsNullOrEmpty(href))
            {
				return new OkObjectResult("Please send href parameter via body or query like " +
					"http://localhost:7015/api/set?href=https://mystat.itstep.org/index");
			}
			var shortUrl = await _hrefProcessingService.RegisterShortUrlAsync(tableClient, href);
			return new OkObjectResult(new { href, shortUrl });
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
			{
				return new BadRequestResult();
			}	
			var result = await _hrefProcessingService.GetShortUrlAsync(shortUrl, tableClient);
			if (result == null)
			{
				return new BadRequestObjectResult("There's no such short URL!");
			}
			await _hrefProcessingService.AddRowKeyToQueueAsync(queue, result.Value.RowKey);
			return new OkObjectResult(result.Value.Url);
		}

		[FunctionName("ProcessQueue")]
		public async Task ProcessQueue(
			[QueueTrigger("counts")] string shortCode,
			[Table("shortUrl")] TableClient tableClient
			)
		{
			await _hrefProcessingService.UpdateHrefObtainCount(shortCode, tableClient);
		}
	}
}
