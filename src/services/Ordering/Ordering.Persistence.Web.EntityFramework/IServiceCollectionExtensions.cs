using Microsoft.Extensions.DependencyInjection;
using System;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddOrderRepository(this IServiceCollection services, Action<MongoOptions> configure)
        {
            var options = new MongoOptions();

            configure(options);

            services.AddMongoOrderRepository(options);

            return services;
        }

        public static IServiceCollection AddMongoOrderRepository(this IServiceCollection services, MongoOptions options)
        {
            services.AddSingleton<IOrdersRepository>(ctx =>
            {
                return new OrdersRepository(options.ConnectionString, options.DatabaseName, options.CollectionName);
            });

            return services;
        }
    }
}
