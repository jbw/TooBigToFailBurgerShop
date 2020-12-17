using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Messages;

namespace TooBigToFailBurgerShop.Ordering.Consumer
{
    public class SubmitBurgerOrderConsumer : IConsumer<SubmitBurgerOrder>
    {
        ILogger<SubmitBurgerOrderConsumer> _logger;

        public SubmitBurgerOrderConsumer(ILogger<SubmitBurgerOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitBurgerOrder> context)
        {
            var message = Create(context.Message.CorrelationId);

            await context.Publish(message);
        }

        private BurgerOrderReceived Create(Guid correlationId)
        {
            return new Received { CorrelationId = correlationId };
        }

        class Received : BurgerOrderReceived
        {
            public Guid CorrelationId { get; set; }
        }
    }

}
