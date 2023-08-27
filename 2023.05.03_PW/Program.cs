using _2023._05._03_PW.Data.Contexts;
using _2023._05._03_PW.Services.LotService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ILotService, LotService>();

builder.Services.AddDbContext<MessagesDataContext>(opt
	=> opt.UseSqlServer(builder.Configuration.GetConnectionString("ImagesLocalDataDB")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAzureClients(clientBuilder =>
{
    //clientBuilder.AddBlobServiceClient(builder.Configuration["AzuriteLocalEmulatorConnectionString:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzuriteLocalEmulatorConnectionString:queue"]!, preferMsi: true);
});

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
