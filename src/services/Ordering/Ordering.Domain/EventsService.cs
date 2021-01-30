using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core
{
    public class EventsService<TType, TKey> : IEventsService<TType, TKey> where TType : class, IAggregateRoot<TKey>
    {
        private readonly IEventProducer<TType, TKey> _eventProducer;
        private readonly IEventsRepository<TType, TKey> _eventRepository;

        public EventsService(IEventProducer<TType, TKey> eventProducer, IEventsRepository<TType, TKey> eventsRepository)
        {
            _eventProducer = eventProducer;
            _eventRepository = eventsRepository;
        }

        public async Task PersistAsync(TType aggregateRoot)
        {
            await _eventRepository.AppendAsync(aggregateRoot);
            await _eventProducer.DispatchAsync(aggregateRoot);

        }
    }
}
