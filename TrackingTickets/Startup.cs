using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackingTickets.Config;
using TrackingTickets.DataAccess;
using TrackingTickets.Repositories.Interfaces;
using TrackingTickets.Repositories;

[assembly: FunctionsStartup(typeof(TrackingTickets.Startup))]

namespace TrackingTickets
{

    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // Configure options from local.settings.json
            builder.ConfigurationBuilder
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;
            builder.Services.Configure<EventHubConfig>(config.GetSection("EventHub"));
            builder.Services.Configure<CosmosDbConfig>(config.GetSection("CosmosDb"));

            builder.Services.AddSingleton<MongoDataAccess>();
            builder.Services.AddTransient<ITrackingRepository, TrackingRepository>();
        }
    }
}
