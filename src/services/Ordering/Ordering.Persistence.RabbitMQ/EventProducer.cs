using MassTransit;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<EventProducer<TType, TKey>> _logger;

        public EventProducer(IPublishEndpoint publishEndpoint, ILogger<EventProducer<TType, TKey>> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task DispatchAsync(TType aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            _logger.LogInformation("publishing " + aggregateRoot.Events.Count + " events for {AggregateId} ...", aggregateRoot.Id);

            // limitation: not generic for other message types yet. 
            foreach (var @event in aggregateRoot.Events)
            {
                var eventType = @event.GetType();
                var message = Convert.ChangeType(@event, eventType);

                await _publishEndpoint.Publish(message);

            }
        }
    }
}
