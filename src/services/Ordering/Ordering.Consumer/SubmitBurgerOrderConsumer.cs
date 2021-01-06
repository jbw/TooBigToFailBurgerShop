using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
            _logger.LogInformation("Order recieved: {0}", context.Message.CorrelationId);


            await Task.FromResult(context.Message);

        }


        class Received : BurgerOrderReceived
        {
            public Guid CorrelationId { get; set; }
        }
    }

}
