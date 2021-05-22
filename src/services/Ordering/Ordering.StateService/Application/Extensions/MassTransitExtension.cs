using Automatonymous.Requests;
using GreenPipes;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Ordering.StateService.Application.Extensions.Dapr;
using System;
using System.Reflection;
using TooBigToFailBurgerShop;
using TooBigToFailBurgerShop.Ordering.State;

namespace Ordering.StateService.Application.Extensions
{
    public static class MassTransitExtension
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {

                x.UsingRabbitMq((context, cfg) =>
                {
                    var rabbitMqSettings = configuration
                        .GetSection(typeof(RabbitMqSettings).Name)
                        .Get<RabbitMqSettings>();

                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                    // Register custom serialiser for Dapr interop
                    var textPlainContentType = new System.Net.Mime.ContentType("text/plain");
                    cfg.AddMessageDeserializer(textPlainContentType, () => new DaprCloudEventTextPlainMessageUnwrapperDeserialiser());
                    
                    // Configure the outbox
                    cfg.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                    cfg.UseMessageRetry(r => r.Immediate(5));

                    cfg.UseInMemoryOutbox();

                    cfg.ConfigureEndpoints(context);

                });

                x.AddSagaStateMachine<BurgerOrderStateMachine, BurgerOrderStateInstance>(cfg =>
                {
                    // Configure the outbox
                    cfg.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                    cfg.UseMessageRetry(r => r.Immediate(5));
                    cfg.UseInMemoryOutbox();

                })
                .EntityFrameworkRepository(r =>
                {
                    r.LockStatementProvider = new PostgresLockStatementProvider();
                    r.ExistingDbContext<BurgerOrderStateDbContext>();

                    r.AddDbContext<DbContext, BurgerOrderStateDbContext>((provider, builder) =>
                    {
                        var connectionString = configuration.GetConnectionString("BurgerOrderStateConnectionString");
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

                });

                x.AddSagaStateMachine<RequestStateMachine, RequestState>(typeof(RequestSagaDefinition))
                    .InMemoryRepository();

            });
        }
    }
}