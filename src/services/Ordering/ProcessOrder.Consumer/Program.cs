using MassTransit;
using Microsoft.Extensions.Hosting;
<<<<<<< HEAD:src/services/Ordering/ProcessOrder.Consumer/Program.cs
using TooBigToFailBurgerShop.ProcessOrder.Application.Extensions;

namespace TooBigToFailBurgerShop.ProcessOrder.Consumer
=======
using TooBigToFailBurgerShop.Ordering.Consumer.Application.Extensions;

namespace TooBigToFailBurgerShop.Ordering.Consumer
>>>>>>> main:src/services/Ordering/Ordering.Consumer/Program.cs
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
