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
using Npgsql;
using CreateOrder.Consumer.Application.Options;

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

                    services.AddOptions();

                    services.Configure<BurgerShopSettings>(configuration.GetSection("BurgerShopSettings"));
                    services.Configure<BurgerShopEventsSettings>(configuration.GetSection("BurgerShopEventsSettings"));
                    services.Configure<OrderIdRepositorySettings>(configuration.GetSection("OrderIdRepositorySettings"));
                    services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));

                    services.AddMassTransitConfiguration(configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>());
                    services.AddMassTransitHostedService();

                    services.AddOpenTelemetryTracing(builder =>
                    {
                        var resourceBuilder = ResourceBuilder
                            .CreateDefault()
                            .AddService(configuration.GetValue<string>("Jaeger:ServiceName"));

                        builder
                            .SetResourceBuilder(resourceBuilder)
                            .AddAspNetCoreInstrumentation()
                            .AddJaegerExporter(options =>
                            {
                                options.AgentHost = configuration.GetValue<string>("Jaeger:Host");
                                options.AgentPort = configuration.GetValue<int>("Jaeger:Port");
                            });
                    });

                    services.AddDbContext<BurgerShopContext>(contextOptions =>
                    {

                        var settings = configuration
                            .GetSection("BurgerShopSettings")
                            .Get<BurgerShopSettings>()
                            .Connection;

                        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                        {
                            Host = settings.Host,
                            Port = settings.Port,
                            Username = settings.Username,
                            Password = settings.Password,
                            Database = settings.Database,

                            AutoPrepareMinUsages = 2,
                            MaxAutoPrepare = 2
                        };

                        var connectionString = connectionStringBuilder.ToString();
                        contextOptions.UseNpgsql(connectionString);

                    });

                    services.AddEventProducer(cfg =>
                    {
                        cfg.AddProducer<Order, Guid>();
                    });

                    services.AddEventStore(cfg =>
                    {

                        var settings = configuration
                          .GetSection("BurgerShopEventsSettings")
                          .Get<BurgerShopEventsSettings>()
                          .Connection;

                        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                        {
                            Host = settings.Host,
                            Port = settings.Port,
                            Username = settings.Username,
                            Password = settings.Password,
                            Database = settings.Database,

                            AutoPrepareMinUsages = 2,
                            MaxAutoPrepare = 2
                        };

                        var connectionString = connectionStringBuilder.ToString();

                        cfg.AddEventStore<Order>(connectionString);
                    });

                    services.AddMongoClient(cfg =>
                    {
                        var options = configuration
                            .GetSection(typeof(OrderIdRepositorySettings).Name)
                            .Get<OrderIdRepositorySettings>()
                            .Connection;

                        cfg.Host = options.Host;
                        cfg.Port = options.Port;
                        cfg.Username = options.Username;
                        cfg.Password = options.Password;
                        cfg.Database = options.Database;
                        cfg.CollectionName = options.CollectionName;
                    });

                    services.AddOrderIdRepository();
                    services.AddOrderArchiveItemRepository();

                })

                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<EventsService<Order, Guid>>().AsImplementedInterfaces();
                });
        }
    }
}