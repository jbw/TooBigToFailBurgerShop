using CreateOrder.Consumer.Application.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using TooBigToFailBurgerShop.Ordering.CreateOrder.Consumer;
using TooBigToFailBurgerShop.Ordering.CreateOrder.Infrastructure;

namespace TooBigToFailBurgerShop.ProcessOrder.Application.Extensions
{
    public static class MassTransitExtensions
    {

        public static void AddMassTransitConfiguration(this IServiceCollection services, RabbitMqSettings rabbitMqSettings)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreateBurgerOrderConsumer>(typeof(CreateBurgerOrderConsumerDefinition));
                x.AddConsumer<OrderArchiverConsumer>(typeof(OrderArchiverConsumerDefinition));

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqSettings.Host, "/", h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }

}
