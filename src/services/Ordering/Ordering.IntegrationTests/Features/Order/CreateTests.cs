using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Persistence.Mongo;
using Xunit;
using Xunit.Abstractions;
using Shouldly;
using System.Net;

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
            _client.DefaultRequestHeaders.Add("x-request-id", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
            _client.DefaultRequestHeaders.Add("jwt-extracted-sub", "90e25abf-b5a3-4899-af34-12bf97d6ce35");
        }


        public class Order
        {
            public Guid OrderId { get; set; }
        }

        [Fact]
        public async Task Should_Return_Validation_Message_When_Missing_CustomerId()
        {
            // Given
            var url = "/api/orders";
            var orderContent = JsonContent.Create(new { });
            _client.DefaultRequestHeaders.Remove("jwt-extracted-sub");

            // When
            var resp = await _client.PutAsync(url, orderContent);

            // Then
            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Validation_Message_When_Invalid_Format_CustomerId()
        {
            // Given
            var url = "/api/orders";
            var orderContent = JsonContent.Create(new { });
            _client.DefaultRequestHeaders.Remove("jwt-extracted-sub");
            _client.DefaultRequestHeaders.Add("jwt-extracted-sub", "invalid");

            // When
            var resp = await _client.PutAsync(url, orderContent);

            // Then
            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Validation_Message_When_Missing_RequestId()
        {
            // Given
            var url = "/api/orders";
            var orderContent = JsonContent.Create(new { });
            _client.DefaultRequestHeaders.Remove("x-request-id");

            // When
            var resp = await _client.PutAsync(url, orderContent);

            // Then
            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Validation_Message_When_Invalid_Format_RequestId()
        {
            // Given
            var url = "/api/orders";
            var orderContent = JsonContent.Create(new { });
            _client.DefaultRequestHeaders.Remove("x-request-id");
            _client.DefaultRequestHeaders.Add("x-request-id", "invalid");

            // When
            var resp = await _client.PutAsync(url, orderContent);

            // Then
            resp.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
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
            
            var content = await resp.Content.ReadFromJsonAsync<Order>();
            var orderId = content.OrderId;
            orderId.ShouldNotBe(default);

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