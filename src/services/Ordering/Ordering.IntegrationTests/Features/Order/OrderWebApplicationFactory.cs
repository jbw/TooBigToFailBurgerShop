using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using MartinCostello.Logging.XUnit;
using Xunit.Abstractions;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Ordering.IntegrationTests.Features.Order
{

    public class OrderWebApplicationFactory : WebApplicationFactory<TooBigToFailBurgerShop.Startup>, ITestOutputHelperAccessor
    {
        public ITestOutputHelper OutputHelper { get; set; }

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            var configuration = GetConfiguration();
            var serviceProviderFactory = new TestServiceProvider();

            return Host.CreateDefaultBuilder()
                .UseSerilog()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseServiceProviderFactory(serviceProviderFactory)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<TooBigToFailBurgerShop.Startup>();
                });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddXUnit(OutputHelper);
            });

            builder.UseEnvironment(Environments.Development);

        }
    }
}
