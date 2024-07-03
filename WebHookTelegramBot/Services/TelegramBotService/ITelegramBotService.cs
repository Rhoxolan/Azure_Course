using Telegram.Bot.Types;

namespace WebHookTelegramBot.Services.TelegramBotService
{
	public interface ITelegramBotService
	{
		Task RespondAsync(Update update);
	}
}
