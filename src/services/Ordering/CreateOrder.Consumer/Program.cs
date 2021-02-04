using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;
using MassTransit;
using TooBigToFailBurgerShop.ProcessOrder.Application.Extensions;
using TooBigToFailBurgerShop.Ordering.Persistence.MassTransit;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using TooBigToFailBurgerShop.Ordering.Persistence.MartenDb;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using Autofac.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Infrastructure;
using TooBigToFailBurgerShop.Ordering.Persistence.Mongo;

namespace TooBigToFailBurgerShop.Ordering.CreateOrder.Consumer
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
                    var configuration = hostContext.Configuration;

                    services.AddMassTransitConfiguration();
                    services.AddMassTransitHostedService();

                    services.AddOpenTelemetryTracing(builder =>
                    {

                        builder
                            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(configuration.GetValue<string>("Jaeger:ServiceName")))
                            .AddAspNetCoreInstrumentation()
                            .AddJaegerExporter(options =>
                            {
                                options.AgentHost = configuration.GetValue<string>("Jaeger:Host");
                                options.AgentPort = configuration.GetValue<int>("Jaeger:Port");
                            });
                    });

                    services.AddDbContext<BurgerShopContext>(contextOptions =>
                        contextOptions.UseNpgsql(configuration.GetConnectionString("BurgerShopConnectionString")));

                    services.AddEventProducer(cfg =>
                    {
                        cfg.AddProducer<Order, Guid>();
                    });

                    services.AddEventStore(cfg =>
                    {
                        var connectionString = configuration.GetConnectionString("BurgerShopEventsConnectionString");
                        cfg.AddEventStore<Order>(connectionString);
                    });

                    services.AddOrderRepository(cfg =>
                    {
                        var options = configuration.GetSection(typeof(OrderIdRepositorySettings).Name).Get<OrderIdRepositorySettings>();

                        cfg.DatabaseName = options.DatabaseName;
                        cfg.CollectionName = options.OrdersCollectionName;
                        cfg.ConnectionString = options.ConnectionString;
                    });

           

                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<EventsService<Order, Guid>>().AsImplementedInterfaces();
                });
        }
    }
}