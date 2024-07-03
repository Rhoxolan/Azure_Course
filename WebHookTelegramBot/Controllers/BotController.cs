using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WebHookTelegramBot.Services.TelegramBotService;

namespace WebHookTelegramBot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BotController(ITelegramBotService telegramBotService) : ControllerBase
	{
		[HttpPost]
		public async Task Post([FromBody]Update update)
		{
			await telegramBotService.RespondAsync(update);
		}
	}
}
