using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
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
            var harness = new InMemoryTestHarness();

            await harness.Start();

            try
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
            finally
            {
                await harness.Stop();
            }
        }
    }
}
