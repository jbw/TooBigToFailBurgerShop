using Microsoft.Extensions.DependencyInjection;
using System;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEventStore(this IServiceCollection services, Action<EventStoreBuilder> builder)
        {
            var eventStoreBuilder = new EventStoreBuilder(services);
            builder(eventStoreBuilder);

            return services;
        }
    }
}
