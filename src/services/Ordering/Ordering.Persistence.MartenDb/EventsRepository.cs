using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{
    public class EventsRepository<TType, TKey> : IEventsRepository<TType, TKey> where TType : class, IAggregateRoot<TKey>
    {
        public Task AppendAsync(TType aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public Task<TType> RehydrateAsync(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
