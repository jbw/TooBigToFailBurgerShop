using Burgers.WebSPA.Authentication;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Burgers.WebSPA.Data
{
    public class OrdersService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public OrdersService(HttpClient client, TokenProvider tokenProvider)
        {
            _httpClient = client;
            _tokenProvider = tokenProvider;

            var token = _tokenProvider.AccessToken;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task CreateOrder()
        {
            var body = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("/api/orders", body);

        }
    }
}
