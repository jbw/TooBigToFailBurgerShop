﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Application.Commands;


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
            var hasRequestGuid = Guid.TryParse(requestId, out Guid requestIdGuid) && requestIdGuid != Guid.Empty;

            if(!hasRequestGuid)
            {
                return BadRequest();
            }

            // Use requestId until we have something more fitting
            createOrderCommand.RequestId = requestIdGuid;

            var requestCreateOrder = new IdempotentCommand<CreateOrderCommand, bool>(createOrderCommand, requestIdGuid);
            var result = await _mediator.Send(requestCreateOrder);

            if (!result) return BadRequest();

            return Ok();
        }
    }
}
