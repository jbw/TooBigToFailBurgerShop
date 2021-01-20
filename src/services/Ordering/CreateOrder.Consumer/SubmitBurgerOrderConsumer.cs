﻿using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Contracts;

namespace TooBigToFailBurgerShop.ProcessOrder.Consumer
{
    public class SubmitBurgerOrderConsumer : IConsumer<SubmitBurgerOrder>
    {
        private readonly ILogger<SubmitBurgerOrderConsumer> _logger;

        public SubmitBurgerOrderConsumer(ILogger<SubmitBurgerOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitBurgerOrder> context)
        {

            _logger.LogInformation($"CreateBurgerOrderConsumer {context.MessageId}");

            // TODO persist the order so it can be processed further down the line.

            // Just republish the exact message for now.
            await context.Publish<BurgerOrderReceived>(context.Message).ConfigureAwait(false);

        }

    }
}
