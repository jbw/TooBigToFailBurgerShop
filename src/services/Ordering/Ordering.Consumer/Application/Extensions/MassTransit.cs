using MassTransit;
using MassTransit.Courier;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.DependencyInjection;
using System;
using TooBigToFailBurgerShop.Ordering.Consumer.Infrastructure;

namespace TooBigToFailBurgerShop.Ordering.Consumer.Application.Extensions
{
    public static class MassTransit
    {

        public static void AddMassTransitConfiguration(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<SubmitBurgerOrderConsumer>(typeof(SubmitBurgerOrderConsumerDefinition));

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
