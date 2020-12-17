using MassTransit;
using Microsoft.Extensions.Hosting;
using TooBigToFailBurgerShop.Ordering.Consumer.Application.Extensions;

namespace TooBigToFailBurgerShop.Ordering.Consumer
{

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransitConfiguration();
                    services.AddMassTransitHostedService();
                });
    }

}
