using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
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

            var jsonContent = JsonSerializer.Serialize(new { UserId = "jbw" });
 
            var body = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("/Orders/createorder", body);
            
        }
    }



}
