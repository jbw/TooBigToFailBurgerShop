using MediatR;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Application.Queries;

using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public class OrdersArchiveHandler : IRequestHandler<OrdersArchive, IEnumerable<OrderArchiveItem>>
    {
        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<OrderArchiveItem> _orderArchiveItemCollection;

        public OrdersArchiveHandler(IMongoClient mongoClient, MongoConnectionSettings mongoOptions)
        {
            _mongoDatabase = mongoClient.GetDatabase(mongoOptions.Database);

            _orderArchiveItemCollection = _mongoDatabase.GetCollection<OrderArchiveItem>(typeof(OrderArchiveItem).Name);
        }


        public async Task<IEnumerable<OrderArchiveItem>> Handle(OrdersArchive request, CancellationToken cancellationToken)
        {
            // perf: this won't last long
            return await _orderArchiveItemCollection
                .Aggregate()
                .ToListAsync(cancellationToken);
        }
    }
}
