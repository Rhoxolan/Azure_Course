using _2023._05._03_PW.Models;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
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

        public async Task<IActionResult> AddLot(CurrencyLot currencyLot)
        {
			QueueClient queueClient = _queueServiceClient.GetQueueClient("lotes");
            await queueClient.CreateIfNotExistsAsync();
			var receipt = await queueClient.SendMessageAsync(JsonSerializer.Serialize(currencyLot), timeToLive: TimeSpan.FromDays(1));
            return Ok(receipt.Value.MessageId);
		}

        public async Task<IActionResult> GetLots(CurrencyType currencyType)
        {
			QueueClient queueClient = _queueServiceClient.GetQueueClient("lotes");
			await queueClient.CreateIfNotExistsAsync();
            //Попробовать Сделать фильтрацию на стороне очереди
			throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}