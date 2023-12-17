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

namespace _2023._05._19_PW
{
    public static class FunctionUrlShortener
    {
        [FunctionName("Set")]
        public static async Task<IActionResult> Run(
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

            return new OkObjectResult("");
        }
    }
}
