using _2023._05._03_PW.Models;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace _2023._05._03_PW.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly QueueServiceClient _queueServiceClient;

		public HomeController(ILogger<HomeController> logger, QueueServiceClient queueServiceClient)
		{
			_logger = logger;
			_queueServiceClient = queueServiceClient;
		}

		public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLot(CurrencyLot currencyLot)
        {
			QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyLot.CurrencyType.ToString().ToLower()}");
            await queueClient.CreateIfNotExistsAsync();
			var receipt = await queueClient.SendMessageAsync(JsonSerializer.Serialize(currencyLot), timeToLive: TimeSpan.FromDays(1));
            return Ok(receipt.Value.MessageId);
		}

        [HttpGet]
        public async Task<IActionResult> GetLots(CurrencyType currencyType)
        {
			QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyType.ToString().ToLower()}");
			await queueClient.CreateIfNotExistsAsync();
            var azureResponse = await queueClient.PeekMessagesAsync(maxMessages: 10);
            return Ok(azureResponse.Value);
        }

        [HttpDelete]
        public async Task<IActionResult> BuyLot(string messageId, CurrencyType currencyType)
        {
            QueueClient queueClient = _queueServiceClient.GetQueueClient($"lotes-{currencyType.ToString().ToLower()}");
            throw new NotImplementedException();

		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}