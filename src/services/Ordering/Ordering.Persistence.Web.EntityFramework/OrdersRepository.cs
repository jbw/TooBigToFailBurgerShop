using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
   
    

    public class OrdersRepository : IOrdersRepository
    {
        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<OrderId> _orderIdCollection;
        private readonly IMongoCollection<OrderArchiveItem> _orderArchiveItemCollection;

        public OrdersRepository(IMongoClient mongoClient, MongoOptions mongoOptions)
        {
           
            _mongoDatabase = mongoClient.GetDatabase(mongoOptions.DatabaseName);

            _orderIdCollection = _mongoDatabase.GetCollection<OrderId>(mongoOptions.CollectionName);
            _orderArchiveItemCollection = _mongoDatabase.GetCollection<OrderArchiveItem>(typeof(OrderArchiveItem).Name);

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

        public async Task CreateAsync(Guid orderId, DateTime timestamp, CancellationToken cancellationToken = default)
        {
            var filter = Builders<OrderArchiveItem>.Filter
                .Eq(a => a.Id, orderId);

            var update = Builders<OrderArchiveItem>.Update
                .Set(a => a.Id, orderId)
                .Set(a => a.Timestamp, timestamp);

            await _orderArchiveItemCollection.UpdateOneAsync(filter,
               cancellationToken: cancellationToken,
               update: update,
               options: new UpdateOptions() { IsUpsert = true }
            );

        }

        public async Task<bool> ExistsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var search = await _orderIdCollection.FindAsync(x => x.Id.Equals(orderId), cancellationToken: cancellationToken);
            return await search.AnyAsync(cancellationToken);
        }
    }
}
