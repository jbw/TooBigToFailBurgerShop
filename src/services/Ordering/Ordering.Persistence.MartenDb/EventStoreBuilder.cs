using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{
    public class EventStoreBuilder
    {
        private IServiceCollection _services;

        public EventStoreBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void AddEventStore<TType>(string connectionString) where TType : class
        {

            _services.AddMarten(cfg =>
            {
                cfg.Connection(connectionString);
                cfg.AutoCreateSchemaObjects = AutoCreate.All;
                cfg.Events.InlineProjections.AggregateStreamsWith<TType>();
            });
        }
    }
}
