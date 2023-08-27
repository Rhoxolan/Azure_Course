using _2023._05._03_PW.Data.Contexts;
using _2023._05._03_PW.Data.Models;
using _2023._05._03_PW.Models;
using _2023._05._03_PW.Services.LotService;
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
        private readonly ILotService _lotService;

		public HomeController(ILogger<HomeController> logger, QueueServiceClient queueServiceClient, MessagesDataContext messagesDataContext, ILotService lotService)
		{
			_logger = logger;
			_queueServiceClient = queueServiceClient;
			_messagesDataContext = messagesDataContext;
			_lotService = lotService;
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
            try
            {
				var receipt = await _lotService.AddLotAsync(currencyLot);
				return Ok(receipt.MessageId);
			}
            catch
            {
				return Problem("Data processing error. Please contact to developer");
			}
		}

        [HttpGet]
        public async Task<IActionResult> GetLots(CurrencyType currencyType)
        {
			try
			{
                var lots = await _lotService.GetLotsAsync(currencyType);
                return Ok(lots);
			}
			catch
			{
				return Problem("Data processing error. Please contact to developer");
			}
        }

        [HttpDelete]
        public async Task<IActionResult> BuyLot(string messageId, CurrencyType currencyType)
        {
            var messageDataEntity = _lotService.GetLotData(messageId, currencyType);
            if(messageDataEntity == null)
            {
                return NotFound();
            }
            await _lotService.BuyLotAsync(messageDataEntity.Value, currencyType);
            return NoContent();

		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}