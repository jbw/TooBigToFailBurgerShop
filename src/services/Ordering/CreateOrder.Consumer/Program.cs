using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Autofac;
using Microsoft.EntityFrameworkCore;
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
using Serilog;
using System.IO;

namespace TooBigToFailBurgerShop.Ordering.CreateOrder.Consumer
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", "Ordering.API");

                var host = CreateHostBuilder(configuration, args).Build();

                Log.Information("Starting web host ({ApplicationContext})...", "Ordering.API");

                await host.RunAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", "Ordering.API");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }


        }

        private static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .ConfigureServices((hostContext, services) =>
                {
                    var configuration = hostContext.Configuration;

                    services.AddOptions();

                    services.Configure<BurgerShopSettings>(configuration.GetSection(nameof(BurgerShopSettings)));
                    services.Configure<BurgerShopEventsSettings>(configuration.GetSection(nameof(BurgerShopEventsSettings)));
                    services.Configure<OrderIdRepositorySettings>(configuration.GetSection(nameof(OrderIdRepositorySettings)));
                    services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));

                    services.AddMassTransitConfiguration(configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>());
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
                            .GetSection(nameof(BurgerShopSettings))
                            .Get<BurgerShopSettings>()
                            .Connection;

                        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                        {
                            Host = settings.Host,
                            Port = settings.Port.Value,
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
                          .GetSection(nameof(BurgerShopEventsSettings))
                          .Get<BurgerShopEventsSettings>()
                          .Connection;

                        var connectionStringBuilder = new NpgsqlConnectionStringBuilder
                        {
                            Host = settings.Host,
                            Port = settings.Port.Value,
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
                        cfg.Port = options.Port.Value;
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

        static IConfiguration GetConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env ?? "Production" }.json", optional: true)
                .AddJsonFile($"appsettings.Logging.json", optional: true)

                .AddEnvironmentVariables();

            return builder.Build();
        }

        static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}