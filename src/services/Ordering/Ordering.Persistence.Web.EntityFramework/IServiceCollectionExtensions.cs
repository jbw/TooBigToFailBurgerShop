using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using TooBigToFailBurgerShop.Ordering.Application.Queries;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoClient(this IServiceCollection services, Action<MongoOptions> configure)
        {
            var options = new MongoOptions();

            configure(options);

            services.AddSingleton(options);

            services.AddSingleton<IMongoClient>(ctx =>
            {
                return new MongoClient(options.ConnectionString);
            });

            return services;
        }

        public static IServiceCollection AddOrderIdRepository(this IServiceCollection services)
        {
            services.AddScoped(c =>
            {
                return c.GetService<IMongoClient>().StartSession();
            });

            services.AddScoped<IOrderIdRepository, OrderIdRepository>();

            return services;
        }

        public static IServiceCollection AddOrderArchiveItemRepository(this IServiceCollection services)
        {
            services.AddScoped(c =>
            {
                return c.GetService<IMongoClient>().StartSession();
            });

            services.AddScoped<IOrderArchiveItemRepository, OrdersArchiveItemRepository>();

            return services;
        }


        public static IServiceCollection AddOrderArchiveByIdHandler(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<OrderArchiveById, OrderArchiveItem>, OrderArchiveByIdHandler>();

            return services;
        }

        public static IServiceCollection AddOrdersArchiveHandler(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<OrdersArchive, IEnumerable<OrderArchiveItem>>, OrdersArchiveHandler>();

            return services;
        }
    }
}