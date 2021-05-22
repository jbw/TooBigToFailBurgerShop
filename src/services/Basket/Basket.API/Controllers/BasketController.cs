using Basket.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly IBasketRepository _basketRepository;

        public BasketController(ILogger<BasketController> logger, IBasketRepository basketRepository)
        {
            _logger = logger;
            _basketRepository = basketRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<CustomerBasket>> Get(
            [FromHeader(Name = "x-request-id")] string requestId,
            [FromHeader(Name = "jwt-extracted-sub")] string customerId)
        {
            _logger.LogInformation("Getting CustomerBasket: {requestId}", requestId);

            var basket = await _basketRepository.GetBasketAsync(customerId);

            if (basket == null)
            {
                return new CustomerBasket(customerId);
            }

            return Ok(basket);
        }


        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
        {
            _logger.LogInformation("Updating CustomerBasket");

            return Ok(await _basketRepository.UpdateBasketAsync(value));
        }

        [HttpDelete("{customerId}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteBasketByIdAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
