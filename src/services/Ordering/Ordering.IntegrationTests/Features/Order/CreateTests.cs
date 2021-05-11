using JohnKnoop.MongoRepository;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TooBigToFailBurgerShop;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;
using TooBigToFailBurgerShop.Ordering.Persistence.Mongo;
using Xunit;
using Xunit.Abstractions;

namespace Ordering.IntegrationTests.Features.Order
{

    public class OrderApiTests 
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public OrderApiTests()
        {
            _configuration = GetConfiguration();
            _client = new HttpClient();
        }

        [Fact]
        public async Task Should_create_new_order()
        {

            // Given
            var url = "http://localhost:16969/Orders/createorder";
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
            var options = _configuration
                           .GetSection(typeof(OrderIdRepositorySettings).Name)
                           .Get<OrderIdRepositorySettings>()
                           .Connection;

            var mongoClientSettings = new MongoClientSettings
            {
                Credential = MongoCredential.CreateCredential(options.Database, options.Username, options.Password),
                Server = new MongoServerAddress(options.Host, (int)options.Port),
                Scheme = ConnectionStringScheme.MongoDB
            };

            IMongoClient mongoClient = new MongoClient(mongoClientSettings);

            MongoRepository.Configure()
                .Database(options.Database, db => db
                    .MapAlongWithSubclassesInSameAssebmly<OrderId>(options.CollectionName)
                    .MapAlongWithSubclassesInSameAssebmly<OrderArchiveItem>())
                .AutoEnlistWithTransactionScopes()
                .Build();

            var repo = new OrdersArchiveItemRepository(mongoClient);
            var id = Guid.NewGuid();
            await repo.CreateAsync(id, DateTime.UtcNow);

            // When
            var getOrderByIdUrl = "http://localhost:16969/Orders/getorder?id=" + id;
            _client.DefaultRequestHeaders.Add("x-requestid", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var resp = await _client.GetAsync(getOrderByIdUrl);

            // Then
            resp.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Should_get_all_orders()
        {
            // Given
            var url = "http://localhost:16969/Orders/getorders";

            // When
            _client.DefaultRequestHeaders.Add("x-requestid", "3fa85f64-5717-4562-b3fc-2c963f66afa6");
            var resp = await _client.GetAsync(url);

            // Then
            resp.EnsureSuccessStatusCode();
        }
    }
}