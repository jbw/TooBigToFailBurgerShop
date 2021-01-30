using System;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public class OrderEventsService : IEventsService<Order, Guid>
    {
        private readonly IEventProducer<Order, Guid> _eventProducer;
        private readonly IEventsRepository<Order, Guid> _eventRepository;

        public OrderEventsService(IEventProducer<Order, Guid> eventProducer, IEventsRepository<Order, Guid> eventsRepository)
        {
            _eventProducer = eventProducer;
            _eventRepository = eventsRepository;
        }

        public async Task PersistAsync(Order aggregateRoot)
        {
            await _eventRepository.AppendAsync(aggregateRoot);
            await _eventProducer.DispatchAsync(aggregateRoot);

        }
    }
}
