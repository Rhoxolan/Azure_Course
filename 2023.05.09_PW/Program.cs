using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostBuilder builder = new HostBuilder();
builder.ConfigureWebJobs(opt =>
{
	opt.AddAzureStorageBlobs();
	opt.AddAzureStorageQueues();
});
builder.ConfigureLogging((HostBuilderContext context, ILoggingBuilder logBuilder) =>
{
	logBuilder.AddConsole();
	logBuilder.AddApplicationInsightsWebJobs(config =>
	{
		config.ConnectionString = context.Configuration.GetSection("APPINSIGHTS_INSTRUMENTATIONCONNECTIONSTRING").Value!;
		config.InstrumentationKey = context.Configuration.GetSection("APPINSIGHTS_INSTRUMENTATIONSKEY").Value!;
	});
});
IHost host = builder.Build();
using (host)
{
	host.Run();
}