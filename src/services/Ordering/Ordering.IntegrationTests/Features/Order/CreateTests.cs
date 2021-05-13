using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Persistence.Mongo;
using Xunit;
using Xunit.Abstractions;
using Shouldly;

namespace Ordering.IntegrationTests.Features.Order
{

    public class OrderApiTests : IClassFixture<OrderWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly OrderWebApplicationFactory _factory;

        public OrderApiTests(OrderWebApplicationFactory factory, ITestOutputHelper outputHelper)
        {
            _factory = factory;
            _factory.OutputHelper = outputHelper;

            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("x-requestid", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
        }

        [Fact]
        public async Task Should_create_new_order()
        {

            // Given
            var url = "/api/orders";
            var orderContent = JsonContent.Create(new { });

            // When
            var resp = await _client.PutAsync(url, orderContent);

            // Then
            resp.EnsureSuccessStatusCode();
            
            var content = await resp.Content.ReadFromJsonAsync<dynamic>();
            var orderId = content["orderId"];
            orderId.ShouldNotBeNull();

        }

        [Fact]
        public async Task Should_get_order_by_id()
        {
            // Given
            OrdersArchiveItemRepository repo = CreateOrderIdRepository();
            var orderId = Guid.NewGuid();
            await repo.CreateAsync(orderId, DateTime.UtcNow);

            // When
            var getOrderByIdUrl = $"api/orders/{orderId}";
            var resp = await _client.GetAsync(getOrderByIdUrl);

            // Then
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Should_get_all_orders()
        {
            // Given
            var url = "/api/orders";

            // When
            var resp = await _client.GetAsync(url);

            // Then
            resp.EnsureSuccessStatusCode();
        }

        private OrdersArchiveItemRepository CreateOrderIdRepository()
        {
            IMongoClient mongoClient = (IMongoClient)_factory.Services.GetService(typeof(IMongoClient));
            var repo = new OrdersArchiveItemRepository(mongoClient);
            return repo;
        }
    }
}