using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Burgers.WebSPA.Data
{
    public class BasketService
    {
        private readonly HttpClient _httpClient;

        // Note: in lieu of an identity server we will use a hardcoded
        // user identifier.
        private readonly string _customerId = "91ee1060-fb77-4df7-9d1f-9134d27162ee";
        public BasketService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task AddItemToCustomerBasket()
        {
            var basket = await GetCustomerBasket();
            var placeholderItem = "Juicy burger";
            basket.Items.Add(new BasketItem(placeholderItem));

            await _httpClient.PostAsJsonAsync<CustomerBasket>("/api/basket", basket);
        }

        public async Task<CustomerBasket> GetCustomerBasket()
        {
            return await _httpClient.GetFromJsonAsync<CustomerBasket>("/api/basket?customerId=" + _customerId);
        }
    }
}
