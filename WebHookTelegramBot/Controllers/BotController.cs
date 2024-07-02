using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebHookTelegramBot.Configuration;

namespace WebHookTelegramBot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BotController() : ControllerBase
	{
		public async Task<IActionResult> Post([FromBody]Update update)
		{
			throw new NotImplementedException();
		}
	}
}
