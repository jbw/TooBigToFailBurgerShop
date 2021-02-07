
using Microsoft.Extensions.DependencyInjection;
using TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MassTransit
{
    public class EventProducerBulder
    {
        private readonly IServiceCollection _services;

        public EventProducerBulder(IServiceCollection services)
        {
            _services = services;
        }

        public void AddProducer<TType, TKey>() where TType : class, IAggregateRoot<TKey>
        {
            _services.AddSingleton<IEventProducer<TType, TKey>, EventProducer<TType, TKey>>();

        }
    }
}