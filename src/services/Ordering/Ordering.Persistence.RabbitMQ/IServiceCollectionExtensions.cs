
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MassTransit
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventProducer(this IServiceCollection services, Action<EventProducerBuilder> build)
        {
            var builder = new EventProducerBuilder(services);

            build?.Invoke(builder);

            return services;
        }
    }
}
