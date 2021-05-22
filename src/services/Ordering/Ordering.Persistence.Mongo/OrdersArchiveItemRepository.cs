using JohnKnoop.MongoRepository;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public class OrdersArchiveItemRepository : IOrderArchiveItemRepository
    {
        private readonly IRepository<OrderArchiveItem> _repository;

        public OrdersArchiveItemRepository(IMongoClient mongoClient)
        {
            _repository = mongoClient.GetRepository<OrderArchiveItem>();

        }

        public async Task CreateAsync(Guid orderId, DateTime timestamp, CancellationToken cancellationToken = default)
        {
            var filter = Builders<OrderArchiveItem>.Filter
                .Eq(a => a.Id, orderId);

            var update = Builders<OrderArchiveItem>.Update
                .Set(a => a.Id, orderId)
                .Set(a => a.Timestamp, timestamp);

            var updatedEntry = await _repository.FindOneAndUpdateAsync(
                x => x.Id.Equals(orderId),
                x => update,
                returnProjection: x => new { x },
                returnedDocumentState: ReturnedDocumentState.AfterUpdate,
                upsert: true
            );

        }

        public async Task<bool> ExistsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var search = await _repository.FindAsync(x => x.Id.Equals(orderId));
            return await search.AnyAsync(cancellationToken);
        }
    }
}
