using Basket.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace Basket.API.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private const string StoreName = "burgers-statestore";

        private readonly ILogger<BasketRepository> _logger;
        private readonly DaprClient _dapr;

        public BasketRepository(ILogger<BasketRepository> logger, DaprClient dapr)
        {
            _logger = logger;
            _dapr = dapr;
        }

        public async Task DeleteBasketAsync(string id)
        {
            await _dapr.DeleteStateAsync(StoreName, id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            return await _dapr.GetStateAsync<CustomerBasket>(StoreName, customerId);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            
            var state = await _dapr.GetStateEntryAsync<CustomerBasket>(StoreName, basket.CustomerId);
            state.Value = basket;

            await state.SaveAsync();

            return await GetBasketAsync(basket.CustomerId);
        }
    }
}
