using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus;

namespace TooBigToFailBurgerShop.Ordering.Persistence.RabbitMQ
{
    public class EventProducer<TType, TKey> : IEventProducer<TType, TKey> where TType : IAggregateRoot<TKey>
    {
        public Task DispatchAsync(TType aggregateKey)
        {
            throw new NotImplementedException();
        }
    }
}
