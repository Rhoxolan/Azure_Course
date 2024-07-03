using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebHookTelegramBot.Services.TelegramBotService
{
	public class TelegramBotService(ITelegramBotClient telegramBotClient) : ITelegramBotService
	{
		public async Task RespondAsync(Update update)
		{
			if (update.Message is not { } message)
			{
				return;
			}

			string messageText = String.Empty;
			if (update.Message?.Type != null)
			{
				messageText = update.Message.Text!;
			}

			long chatId = message.Chat.Id;
			await telegramBotClient.SendTextMessageAsync(chatId, $"Bot's Answer on the message '{messageText}'");
		}
	}
}
