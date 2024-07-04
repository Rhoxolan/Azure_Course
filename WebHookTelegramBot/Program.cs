using Microsoft.Extensions.Options;
using Telegram.Bot;
using WebHookTelegramBot.Configuration;
using WebHookTelegramBot.Services.TelegramBotService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BotConfiguration>(builder.Configuration);

builder.Services.AddHttpClient("telegram_bot_client")
	.AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
	{
		var botConfig = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;
		TelegramBotClientOptions options = new(botConfig.BotToken);
		return new TelegramBotClient(options, httpClient);
	});

builder.Services.AddTransient<ITelegramBotService, TelegramBotService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
