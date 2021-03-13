using JohnKnoop.MongoRepository;
using JohnKnoop.MongoRepository.DotNetCoreDi;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.Collections.Generic;
using TooBigToFailBurgerShop.Ordering.Application.Queries;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoClient(this IServiceCollection services, Action<MongoConnectionSettings> configure)
        {
            var options = new MongoConnectionSettings();

            configure?.Invoke(options);

            services.AddSingleton(options);

            var mongoClientSettings = new MongoClientSettings
            {
                Credential = MongoCredential.CreateCredential(options.Database, options.Username, options.Password),
                Server = new MongoServerAddress(options.Host, options.Port),
                Scheme = ConnectionStringScheme.MongoDB
            };

            var mongoClient = new MongoClient(mongoClientSettings);

            services.AddSingleton<IMongoClient>(mongoClient);

            MongoRepository.Configure()
                .Database(options.Database, db => db
                    .MapAlongWithSubclassesInSameAssebmly<OrderId>(options.CollectionName)
                    .MapAlongWithSubclassesInSameAssebmly<OrderArchiveItem>())
                .AutoEnlistWithTransactionScopes()
                .Build();

            services.AddRepositories(mongoClient);

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