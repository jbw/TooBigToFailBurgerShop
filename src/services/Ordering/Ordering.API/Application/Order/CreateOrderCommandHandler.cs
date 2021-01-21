using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Infrastructure.Idempotency;
using TooBigToFailBurgerShop.Ordering.Contracts;

namespace TooBigToFailBurgerShop.Application.Commands.Order
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CreateOrderCommand> _logger;

        public CreateOrderCommandHandler(IPublishEndpoint publishEndpoint, ILogger<CreateOrderCommand> logger)
        {
            _publishEndpoint = publishEndpoint;
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

            var message = new
            {
                OrderDate = DateTime.UtcNow,
                OrderId = InVar.Id,
                CorrelationId = InVar.Id,

            };

            // Submit the burger order as a message and we're done. 
            await _publishEndpoint.Publish<SubmitBurgerOrder>(message, cancellationToken);

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
