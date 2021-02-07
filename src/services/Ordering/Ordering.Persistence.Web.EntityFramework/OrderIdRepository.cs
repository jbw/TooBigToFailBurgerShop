using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public class OrderIdRepository : IOrderIdRepository
    {
        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<OrderId> _orderIdCollection;

        public OrderIdRepository(IMongoClient mongoClient, MongoOptions mongoOptions)
        {

            _mongoDatabase = mongoClient.GetDatabase(mongoOptions.DatabaseName);

            _orderIdCollection = _mongoDatabase.GetCollection<OrderId>(mongoOptions.CollectionName);

        }

        public async Task CreateAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var update = Builders<OrderId>.Update
                 .Set(a => a.Id, orderId);

            await _orderIdCollection.UpdateOneAsync(
                c => c.Id == orderId,
                update,
                options: new UpdateOptions() { IsUpsert = true },
                cancellationToken
            );
        }

        public async Task<bool> ExistsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var search = await _orderIdCollection.FindAsync(x => x.Id.Equals(orderId), cancellationToken: cancellationToken);
            return await search.AnyAsync(cancellationToken);
        }
    }
}
