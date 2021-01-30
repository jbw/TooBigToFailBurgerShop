using Marten;
using System;
using System.Linq;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{
    public class EventsRepository<TType, TKey> : IEventsRepository<TType, TKey> where TType : class, IAggregateRoot<TKey>
    {
        private readonly IDocumentStore _store;

        public EventsRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task AppendAsync(TType aggregateRoot)
        {
            using var session = _store.OpenSession();

            Ordering.Domain.Events.OrderCreated e = (Domain.Events.OrderCreated)aggregateRoot.Events.First();
            session.Events.Append(e.AggregateId, e);

            await session.SaveChangesAsync();
        }

        public Task<TType> RehydrateAsync(TKey key)
        {
            throw new NotImplementedException();
        }
    }
}
