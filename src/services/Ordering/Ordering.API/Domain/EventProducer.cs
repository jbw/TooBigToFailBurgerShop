using System;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public class EventProducer<TType, TKey> : IEventProducer<TType, TKey> where TType : IAggregateRoot<TKey>
    {
        public Task DispatchAsync(TType aggregateKey)
        {
            throw new NotImplementedException();
        }
    }
}
