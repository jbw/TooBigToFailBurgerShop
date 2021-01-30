using Marten;
using System;
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

            var aggregateRootId = Guid.Parse(aggregateRoot.Id.ToString());
            var aggregateVersion = aggregateRoot.Version;
            var events = aggregateRoot.Events;

            foreach (var @event in events)
            {

                session.Events.Append(aggregateRootId, aggregateRoot, @event);
            }

            await session.SaveChangesAsync();
        }

        public Task<TType> RehydrateAsync(TKey key)
        {
            throw new NotImplementedException();
        }
    }

    public class EventSerializer
    {

    }
}
