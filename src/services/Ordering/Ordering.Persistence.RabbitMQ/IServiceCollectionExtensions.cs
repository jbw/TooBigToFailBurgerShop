
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MassTransit
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventProducer(this IServiceCollection services, Action<EventProducerBulder> build)
        {
            var builder = new EventProducerBulder(services);

            build(builder);

            return services;
        }

      
    }
}
