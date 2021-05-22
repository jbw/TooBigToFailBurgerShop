
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MassTransit
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventProducer(this IServiceCollection services, Action<EventProducerBuilder> builder)
        {
            var eventProducerBuilder = new EventProducerBuilder(services);

            builder?.Invoke(eventProducerBuilder);

            return services;
        }
    }
}
