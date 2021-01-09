using Automatonymous.Requests;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using TooBigToFailBurgerShop.Ordering.State;

namespace Ordering.StateService
{
    public static class MassTransitExtension
    {
        public static void AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {

                x.AddSagaStateMachine<BurgerOrderStateMachine, BurgerOrderStateInstance>()
                    .InMemoryRepository();

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