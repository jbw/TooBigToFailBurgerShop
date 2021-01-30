using Marten;
using Marten.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMartenEventsInfrastructure<TType>(this IServiceCollection services, string connectionString) where TType : class
        {

            services.AddMarten(cfg =>
            {
                cfg.Connection(connectionString);

                cfg.AutoCreateSchemaObjects = AutoCreate.All;

                cfg.Events.AddEventTypes(new[] { 
                    typeof(Domain.Events.OrderCreated)
                });

    
                cfg.Events.InlineProjections.AggregateStreamsWith<Domain.AggregatesModel.Order>();
            });

            return services;
        }
    }
}
