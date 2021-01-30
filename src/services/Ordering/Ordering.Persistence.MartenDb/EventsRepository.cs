using Marten;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{

    public class EventsRepository<TType> : IEventsRepository<TType, Guid> where TType : class, IAggregateRoot<Guid>
    {
        private readonly IDocumentStore _store;

        public EventsRepository(IDocumentStore store)
        {
            _store = store;
        }

        public async Task AppendAsync(TType aggregateRoot)
        {
            using var session = _store.OpenSession();

            var aggregateRootId = aggregateRoot.Id;
            int aggregateVersion = (int)aggregateRoot.Version;
            var events = aggregateRoot.Events;

            foreach (var @event in events)
            {
                session.Events.Append(aggregateRootId, aggregateVersion, @event);
            }

            await session.SaveChangesAsync();
        }

        public Task<TType> RehydrateAsync(Guid key)
        {
            throw new NotImplementedException();
        }
    }

    public class EventSerializer
    {

    }
}
