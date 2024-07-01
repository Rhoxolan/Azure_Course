using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebHookTelegramBot.Configuration;

namespace WebHookTelegramBot.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BotController(BotConfiguration botConfiguration) : ControllerBase
	{
	}
}
