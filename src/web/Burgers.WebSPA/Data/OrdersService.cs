using System;
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

        public async Task CreateOrder()
        {
            _httpClient.DefaultRequestHeaders.Remove("x-requestid");
            _httpClient.DefaultRequestHeaders.Add("x-requestid", Guid.NewGuid().ToString());

            var body = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("/api/orders", body);

        }
    }
}
