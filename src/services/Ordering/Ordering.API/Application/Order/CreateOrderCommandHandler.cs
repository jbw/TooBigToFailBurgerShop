using Dapr.Client;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Infrastructure.Idempotency;

namespace TooBigToFailBurgerShop.Application.Commands.Order
{

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private const string DaprPubSubName = "burgers-pubsub";
        private readonly DaprClient _dapr;
        private readonly ILogger<CreateOrderCommand> _logger;


        public CreateOrderCommandHandler(DaprClient dapr, ILogger<CreateOrderCommand> logger)
        {
            _dapr = dapr;
            _logger = logger;
        }

        /// <summary>
        /// Handles a request to create an Order.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Ordering.API, CreateOrderCommandHandler");

            var correlationId = Guid.NewGuid();

            var message = new
            {
                CorrelationId = correlationId,
                Message = new
                {
                    OrderDate = DateTime.UtcNow,
                    OrderId = request.OrderId,
                    CustomerId = request.CustomerId,
                    CorrelationId = correlationId
                },

                MessageType = new string[]
                {
                    "urn:message:TooBigToFailBurgerShop.Ordering.Contracts:SubmitBurgerOrder"
                }
            };

            var topic = "TooBigToFailBurgerShop.Ordering.Contracts:SubmitBurgerOrder";

            await _dapr.PublishEventAsync(
                DaprPubSubName,
                topic,
                message,
                cancellationToken);

            return true;
        }
    }

    // Use for Idempotency in Command process
    public class CreateOrderIdempotentCommandHandler : IdempotentCommandHandler<CreateOrderCommand, bool>
    {
        public CreateOrderIdempotentCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdempotentCommandHandler<CreateOrderCommand, bool>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override bool CreateResultForDuplicateRequest()
        {
            return true; // Ignore duplicate requests for creating order.
        }
    }
}
