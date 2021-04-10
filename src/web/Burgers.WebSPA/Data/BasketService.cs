using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
namespace Burgers.WebSPA.Data
{
    public class BasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task AddItemToCustomerBasket()
        {
            _httpClient.DefaultRequestHeaders.Remove("x-requestid");
            _httpClient.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());


            var customerId = Guid.NewGuid().ToString();
            var items = new List<string> { };

            var body = new StringContent(
                JsonSerializer.Serialize(new { customerId, items }), 
                System.Text.Encoding.UTF8, 
                "application/json"
            );

            await _httpClient.PostAsync("/Basket", body);

        }
    }

}
