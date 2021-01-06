using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Burgers.WebSPA.Data
{
    public class OrdersService
    {
        private readonly HttpClient _httpClient;

        public OrdersService(HttpClient client)
        {
            _httpClient = client;
        }

        public Task<Order> CreateOrder()
        { 
            return Task.FromResult(new Order { Name = "Hello" });
        }
    }
}
