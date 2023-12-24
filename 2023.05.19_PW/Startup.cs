using _2023._05._19_PW.Services.HrefProcessingService;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(_2023._05._19_PW.Startup))]

namespace _2023._05._19_PW
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			builder.Services.AddTransient<IHrefProcessingService, HrefProcessingService>();
		}
	}
}
