using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;
using TooBigToFailBurgerShop.Ordering.Domain;
using JohnKnoop.MongoRepository;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public class OrderIdRepository : IOrderIdRepository
    {
        private readonly IRepository<OrderId> _repository;
        private readonly MongoOptions _mongoOptions;


        public OrderIdRepository(IMongoClient mongoClient, MongoOptions mongoOptions)
        {
            _repository = mongoClient.GetRepository<OrderId>();
            _mongoOptions = mongoOptions;
        }

        public async Task CreateAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            //_repository.EnlistWithCurrentTransactionScope();
            await _repository.WithTransactionAsync(async () =>
            {

                var update = Builders<OrderId>.Update
                     .Set(a => a.Id, orderId);

                await _repository.UpdateOneAsync(
                    c => c.Id == orderId,
                    x => x.Set(y => y.Id, orderId),
                    options: new UpdateOptions() { IsUpsert = true }
                );

            }, TransactionType.TransactionScope, maxRetries: 3);
        }


        public async Task<bool> ExistsAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var search = await _repository.FindAsync(x => x.Id.Equals(orderId));
            return await search.AnyAsync(cancellationToken);
        }
    }
}
