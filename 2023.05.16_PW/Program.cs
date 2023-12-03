using _2023._05._16_PW.Filters;
using _2023._05._16_PW.Infrastructure.Services.CatCosmosService;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICatsCosmosService, CatsCosmosService>(opt =>
{
	IConfigurationSection cosmosSection = builder.Configuration.GetSection("AzureCosmosDBSettings");
	var uri = cosmosSection.GetValue<string>("URI");
	var primaryKey = cosmosSection.GetValue<string>("PrimaryKey");
	var darabaseName = cosmosSection.GetValue<string>("DBName");
	var containerName = cosmosSection.GetValue<string>("ContainerName");
	CosmosClient cosmosClient = new(uri, primaryKey);
	return new CatsCosmosService(cosmosClient, darabaseName!, containerName!);
});

builder.Services.AddScoped<ApplicationExceptionFilterAttribute>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
