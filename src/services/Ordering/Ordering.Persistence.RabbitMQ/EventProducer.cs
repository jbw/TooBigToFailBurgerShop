using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus;
using TooBigToFailBurgerShop.Ordering.Domain.Events;

namespace TooBigToFailBurgerShop.Ordering.Persistence.RabbitMQ
{
    public class Envelope
    {
        public Guid Key { get; set; }
        public string Value { get; set; }
    }

    public class EventProducer<TType, TKey> : IEventProducer<TType, TKey> where TType : IAggregateRoot<TKey>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventProducer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task DispatchAsync(TType aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            // limitation: not generic for other message types yet. 
            foreach (var @event in aggregateRoot.Events)
            {

                var message = new 
                {
                    AggregateId = @event.AggregateId,
                    Timestamp = DateTime.UtcNow,
                    AggregateVersion = @event.AggregateVersion
                };

                await _publishEndpoint.Publish<OrderCreated>(message);

            }
        }
    }
}
