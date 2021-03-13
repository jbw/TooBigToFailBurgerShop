using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Application.Queries;

using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{

    public class OrderArchiveByIdHandler : IRequestHandler<OrderArchiveById, OrderArchiveItem>
    {
        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<OrderArchiveItem> _orderArchiveItemCollection;

        public OrderArchiveByIdHandler(IMongoClient mongoClient, MongoConnectionSettings mongoOptions)
        {
            _mongoDatabase = mongoClient.GetDatabase(mongoOptions.Database);

            _orderArchiveItemCollection = _mongoDatabase.GetCollection<OrderArchiveItem>(typeof(OrderArchiveItem).Name);
        }

        public async Task<OrderArchiveItem> Handle(OrderArchiveById request, CancellationToken cancellationToken)
        {
            var cursor = await _orderArchiveItemCollection.FindAsync(c => c.Id == request.Id, null, cancellationToken);

            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
