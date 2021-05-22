using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Application.Commands.Order;
using TooBigToFailBurgerShop.Ordering.Application.Queries;
using TooBigToFailBurgerShop.Ordering.Infrastructure.Idempotency;

namespace TooBigToFailBurgerShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {

        private readonly ILogger<OrdersController> _logger;
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrderAsync(
            [FromBody] CreateOrderCommand createOrderCommand,
            [FromHeader(Name = "x-request-id")] string requestId,
            [FromHeader(Name = "jwt-extracted-sub")] string customerId)
        {
            _logger.LogInformation("CreateOrderAsync: {requestId}", requestId);

            var hasRequestGuid = Guid.TryParse(requestId, out Guid requestIdGuid) && requestIdGuid != Guid.Empty;
            if (!hasRequestGuid) return BadRequest();

            var hasCustomerGuid = Guid.TryParse(customerId, out Guid customerIdGuid) && customerIdGuid != Guid.Empty;
            if (!hasCustomerGuid) return BadRequest();

            createOrderCommand.OrderId = requestIdGuid;
            createOrderCommand.CustomerId = customerIdGuid;

            var requestCreateOrder = new IdempotentCommand<CreateOrderCommand, bool>(createOrderCommand, requestIdGuid);

            var result = await _mediator.Send(requestCreateOrder);

            if (!result) return BadRequest();

            return Ok(new { OrderId = requestCreateOrder.Command.OrderId });
        }



        [HttpGet("{id:Guid}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("GetOrderAsync: {id}", id);

            var query = new OrderArchiveById(id);

            var result = await _mediator.Send(query, cancellationToken);

            if (result == null) return NotFound();

            return Ok(result);
        }


        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(nameof(GetOrderAsync));

            var query = new OrdersArchive();

            var result = await _mediator.Send(query, cancellationToken);

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
