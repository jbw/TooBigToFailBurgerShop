using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Contracts;

namespace TooBigToFailBurgerShop.ProcessOrder.Consumer
{
    public class CreateBurgerOrderConsumer : IConsumer<CreateBurgerOrder>
    {
        private readonly ILogger<CreateBurgerOrderConsumer> _logger;

        public CreateBurgerOrderConsumer(ILogger<CreateBurgerOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateBurgerOrder> context)
        {
            _logger.LogInformation($"CreateBurgerOrderConsumer {context.Message.CorrelationId}");

            await context.Publish<BurgerOrderReceived>(context.Message).ConfigureAwait(false);

        }

    }
}
