using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Application.Commands.Order;
using TooBigToFailBurgerShop.Application.Queries;
using TooBigToFailBurgerShop.Ordering.Infrastructure.Idempotency;

namespace TooBigToFailBurgerShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
   
        private readonly ILogger<OrdersController> _logger;
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [Route("createorder")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrderAsync([FromBody]CreateOrderCommand createOrderCommand, [FromHeader(Name = "x-requestid")] string requestId)
        {

            _logger.LogInformation("CreateOrderAsync: {requestId}", requestId);

            var hasRequestGuid = Guid.TryParse(requestId, out Guid requestIdGuid) && requestIdGuid != Guid.Empty;

            if(!hasRequestGuid)
            {
                return BadRequest();
            }

            var requestCreateOrder = new IdempotentCommand<CreateOrderCommand, bool>(createOrderCommand, requestIdGuid);

            var result = await _mediator.Send(requestCreateOrder);

            if (!result) return BadRequest();

            return Ok();
        }



        [Route("getorder")]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderAsync(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("GetOrderAsync: {id}", id);

            var query = new OrderById(id);

            var result = await _mediator.Send(query, cancellationToken);

            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}
