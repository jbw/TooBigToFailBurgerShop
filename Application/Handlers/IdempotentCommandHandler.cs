using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Infrastructure.Idempotency;

namespace TooBigToFailBurgerShop.Application.Commands
{
    public class IdempotentCommandHandler<T, R> : IRequestHandler<IdempotentCommand<T, R>, R> where T : IRequest<R>
    {
        private readonly IMediator _mediator;
        private readonly IRequestManager _requestManager;
        private readonly ILogger<IdempotentCommandHandler<T, R>> _logger;

        public IdempotentCommandHandler(IMediator mediator, IRequestManager requestManager, ILogger<IdempotentCommandHandler<T,R>> logger)
        {
            _mediator = mediator;
            _requestManager = requestManager;
            _logger = logger;
        }

        public async Task<R> Handle(IdempotentCommand<T, R> request, CancellationToken cancellationToken)
        {
            // Check not a duplicate request

            var result = await _mediator.Send(request.Command, cancellationToken);

            return result;
        }


        protected virtual bool CreateResultForDuplicateRequest()
        {
            return true;                // Ignore duplicate requests for creating order.
        }
    }
}
