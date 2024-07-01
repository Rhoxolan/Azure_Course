using WebHookTelegramBot.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<BotConfiguration>(builder.Configuration);

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
