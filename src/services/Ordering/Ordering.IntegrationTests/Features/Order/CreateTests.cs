using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Persistence.Mongo;
using Xunit;
using Xunit.Abstractions;

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

            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Should_create_new_order()
        {

            // Given
            var url = "/Orders/createorder";
            var orderContent = JsonContent.Create(new { });

            // When
            _client.DefaultRequestHeaders.Add("x-requestid", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var resp = await _client.PutAsync(url, orderContent);

            // Then
            resp.EnsureSuccessStatusCode();

        }

        [Fact]
        public async Task Should_get_order_by_id()
        {
            // Given
            IMongoClient mongoClient = (IMongoClient)_factory.Services.GetService(typeof(IMongoClient));
            var repo = new OrdersArchiveItemRepository(mongoClient);
            var id = Guid.NewGuid();
            await repo.CreateAsync(id, DateTime.UtcNow);

            // When
            var getOrderByIdUrl = "/Orders/getorder?id=" + id;
            _client.DefaultRequestHeaders.Add("x-requestid", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var resp = await _client.GetAsync(getOrderByIdUrl);

            // Then
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Should_get_all_orders()
        {
            // Given
            var url = "/Orders/getorders";

            // When
            _client.DefaultRequestHeaders.Add("x-requestid", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var resp = await _client.GetAsync(url);

            // Then
            resp.EnsureSuccessStatusCode();
        }
    }
}