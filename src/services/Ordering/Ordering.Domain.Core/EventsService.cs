using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

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

            // Info: Here we have decoupled the raising of domain events 
            // and persiting the aggregate. The event raising and handling is 
            // also decoupled and handled by TType events consumers. 
            // E.g Order aggregate events (OrderCreated)

            // Dispatch the events and then commit the data.
            await _eventProducer.DispatchAsync(aggregateRoot);
            await _eventRepository.AppendAsync(aggregateRoot);

            aggregateRoot.ClearEvents();

        }
    }
}
