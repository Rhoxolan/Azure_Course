using Azure.Data.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;

namespace _2023._05._19_PW.Services.HrefProcessingService
{
	public interface IHrefProcessingService
	{
		Task<string?> GetHrefFromHttpRequestAsync(HttpRequest req);

		Task<string> RegisterShortUrlAsync(TableClient tableClient, string href);

		Task<(string RowKey, string Url)?> GetShortUrlAsync(string shortUrl, TableClient tableClient);

		Task AddRowKeyToQueueAsync(IAsyncCollector<string> queue, string rowKey);

		Task UpdateHrefObtainCount(string shortCode, TableClient tableClient);
	}
}
