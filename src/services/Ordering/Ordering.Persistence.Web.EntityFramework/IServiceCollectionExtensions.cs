using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using TooBigToFailBurgerShop.Application.Queries;
using TooBigToFailBurgerShop.Application.Queries.Models;
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
            services.AddSingleton<IMongoClient>(ctx =>
            {
                return new MongoClient(options.ConnectionString);
            });

            services.AddSingleton(options);

            services.AddScoped(c =>
            {
                return c.GetService<IMongoClient>().StartSession();
            });

            services.AddScoped<IOrdersRepository, OrdersRepository>();

            services.AddScoped<IRequestHandler<OrderById, OrderDetails>, OrderByIdHandler>();
     

            return services;
        }
    }
}
