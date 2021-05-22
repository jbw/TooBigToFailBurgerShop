using Marten;
using Microsoft.Extensions.DependencyInjection;
using System;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{
    public class EventStoreBuilder
    {
        private IServiceCollection _services;

        public EventStoreBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void AddEventStore<TType>(string connectionString) where TType : class, IAggregateRoot<Guid>
        {
            _services.AddMarten(cfg =>
            {
                cfg.Connection(connectionString);
                cfg.AutoCreateSchemaObjects = AutoCreate.All;
                cfg.DatabaseSchemaName = Marten.StoreOptions.DefaultDatabaseSchemaName;

            });
       
            _services.AddSingleton<IEventsRepository<TType, Guid>, EventsRepository<TType>>();
        }
    }
}
