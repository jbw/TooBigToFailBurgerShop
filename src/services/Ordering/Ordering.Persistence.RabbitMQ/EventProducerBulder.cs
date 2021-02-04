
using Microsoft.Extensions.DependencyInjection;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MassTransit
{
    public class EventProducerBulder
    {
        private readonly IServiceCollection _services;

        public EventProducerBulder(IServiceCollection services)
        {
            _services = services;
        }

        public void AddEventProducer<TType, TKey>() where TType : IAggregateRoot<TKey>
        {
            _services.AddSingleton<IEventProducer<TType, TKey>>();

        }
    }
}
