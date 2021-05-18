using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Burgers.WebSPA.Authentication;

namespace Burgers.WebSPA.Data
{
    public class BasketService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public BasketService(HttpClient client, TokenProvider tokenProvider)
        {
            _httpClient = client;
            _tokenProvider = tokenProvider;

            var token = _tokenProvider.AccessToken;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        public async Task AddItemToCustomerBasket()
        {
            
            var basket = await GetCustomerBasket();
            var placeholderItem = "Juicy burger";
            basket.Items.Add(new BasketItem(placeholderItem));

            await _httpClient.PostAsJsonAsync("/api/basket", basket);
        }

        public async Task<CustomerBasket> GetCustomerBasket( )
        {
            return await _httpClient.GetFromJsonAsync<CustomerBasket>($"/api/basket");
        }
    }
}
