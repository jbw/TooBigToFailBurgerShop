using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Ordering.StateService;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;


namespace TooBigToFailBurgerShop.Ordering.StateService
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<BurgerOrderStateDbContext>(builder =>
                    {
                        var connectionString = hostContext.Configuration.GetConnectionString("BurgerOrderStateConnectionString");
                        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
                        {
                            AutoPrepareMinUsages = 2,
                            MaxAutoPrepare = 2
                        };

                        connectionString = connectionStringBuilder.ToString();

                        builder.UseNpgsql(connectionString, m =>
                        {
    
                            m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                            m.MigrationsHistoryTable($"__{nameof(BurgerOrderStateDbContext)}");
                        });
                    });

                    services.AddMassTransitHostedService();
                    services.AddMassTransitConfiguration(hostContext.Configuration);
                });
        }
    }
}

