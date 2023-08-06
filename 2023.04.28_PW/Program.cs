using _2023._04._28_PW.Data.Contexts;
using _2023._04._28_PW.Services.BlobService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ImagesContext>(opt
	=> opt.UseSqlServer(builder.Configuration.GetConnectionString("ImagesLocalDataDB")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAzureClients(clientBuilder =>
{
	clientBuilder.AddBlobServiceClient(Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING"));
});

builder.Services.AddTransient<IBlobService, BlobService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
