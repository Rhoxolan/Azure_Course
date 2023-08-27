using _2023._05._03_PW.Data.Contexts;
using _2023._05._03_PW.Data.Models;
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
        private readonly MessagesDataContext _messagesDataContext;

		public HomeController(ILogger<HomeController> logger, QueueServiceClient queueServiceClient, MessagesDataContext messagesDataContext)
		{
			_logger = logger;
			_queueServiceClient = queueServiceClient;
			_messagesDataContext = messagesDataContext;
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
            var messageDataEntity = new MessageDataEntity
            {
                MessageId = receipt.Value.MessageId,
                PopReceipt = receipt.Value.PopReceipt
            };
            await _messagesDataContext.MessageDataEntities.AddAsync(messageDataEntity);
            await _messagesDataContext.SaveChangesAsync();
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
            var messageDataEntity = _messagesDataContext.MessageDataEntities.FirstOrDefault(m => m.MessageId == messageId);
            if(messageDataEntity == null)
            {
                return NotFound();
            }
			await queueClient.DeleteMessageAsync(messageId, messageDataEntity.PopReceipt);
            _messagesDataContext.MessageDataEntities.Remove(messageDataEntity);
            await _messagesDataContext.SaveChangesAsync();
            return NoContent();

		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}