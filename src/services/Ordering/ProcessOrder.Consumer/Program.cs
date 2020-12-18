using MassTransit;
using Microsoft.Extensions.Hosting;
using TooBigToFailBurgerShop.ProcessOrder.Application.Extensions;

namespace TooBigToFailBurgerShop.ProcessOrder.Consumer
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
