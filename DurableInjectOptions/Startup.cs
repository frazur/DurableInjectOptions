using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(DurableInjectOptions.Startup))]

namespace DurableInjectOptions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<IMyConfig>().Configure<IConfiguration>(
                (settings, configuration) => {
                    configuration.GetSection("MyConfig").Bind(settings);
                });
        }
    }
}
