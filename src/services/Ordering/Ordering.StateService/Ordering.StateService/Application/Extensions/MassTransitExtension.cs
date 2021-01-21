using Automatonymous.Requests;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TooBigToFailBurgerShop.Ordering.State;

namespace Ordering.StateService
{
    public static class MassTransitExtension
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddSagaStateMachine<BurgerOrderStateMachine, BurgerOrderStateInstance>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.LockStatementProvider = new PostgresLockStatementProvider();
                        r.ExistingDbContext<BurgerOrderStateDbContext>();

                        r.AddDbContext<DbContext, BurgerOrderStateDbContext>((provider, builder) =>
                        {
                            var connectionString = configuration.GetConnectionString("BurgerOrderStateConnectionString");

                            builder.UseNpgsql(connectionString, m =>
                            {
                                m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                m.MigrationsHistoryTable($"__{nameof(BurgerOrderStateDbContext)}");
                            });
                        });

                    });

                x.AddSagaStateMachine<RequestStateMachine, RequestState>(typeof(RequestSagaDefinition))
                    .InMemoryRepository();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);

                    cfg.UseInMemoryOutbox();

                });
            });
        }
    }
}