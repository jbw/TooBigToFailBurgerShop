using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{

    public class EventsRepository<TType> : IEventsRepository<TType, Guid> where TType : class, IAggregateRoot<Guid>
    {
        private readonly IDocumentStore _store;
        private readonly ILogger<EventsRepository<TType>> _logger;

        public EventsRepository(IDocumentStore store, ILogger<EventsRepository<TType>> logger)
        {
            _store = store;
            _logger = logger;
        }

        public async Task AppendAsync(TType aggregateRoot)
        {
            _logger.LogInformation("EventsRepository {id}", aggregateRoot.Id);

            using var session = _store.OpenSession(SessionOptions.ForCurrentTransaction());

            var aggregateRootId = aggregateRoot.Id;
            int aggregateVersion = (int)aggregateRoot.Version;
            var events = aggregateRoot.Events;

            session.Events.StartStream<TType>(aggregateRootId, events);

            await session.SaveChangesAsync();

        }

        public Task<TType> RehydrateAsync(Guid key)
        {
            throw new NotImplementedException();
        }
    }
}
