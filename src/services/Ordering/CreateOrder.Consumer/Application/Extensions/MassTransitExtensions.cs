using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using TooBigToFailBurgerShop.Ordering.CreateOrder.Consumer;
using TooBigToFailBurgerShop.Ordering.CreateOrder.Infrastructure;

namespace TooBigToFailBurgerShop.ProcessOrder.Application.Extensions
{
    public static class MassTransitExtensions
    {
   
        public static void AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateBurgerOrderConsumer>(typeof(CreateBurgerOrderConsumerDefinition));
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }

}
