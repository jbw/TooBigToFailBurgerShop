using MediatR;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Application.Queries;
using TooBigToFailBurgerShop.Application.Queries.Models;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    public class OrderByIdHandler : IRequestHandler<OrderById, OrderDetails>
    {
        private readonly IMongoDatabase _mongoDatabase;

        private readonly IMongoCollection<OrderArchiveItem> _orderArchiveItemCollection;

        public OrderByIdHandler(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _mongoDatabase = client.GetDatabase(databaseName);

            _orderArchiveItemCollection = _mongoDatabase.GetCollection<OrderArchiveItem>(typeof(OrderArchiveItem).Name);
        }

        public async Task<OrderDetails> Handle(OrderById request, CancellationToken cancellationToken)
        {
            var cursor = await _orderArchiveItemCollection.FindAsync(c => c.Id == request.Id, null, cancellationToken);
            var order = await cursor.FirstOrDefaultAsync(cancellationToken);

            if (order == null)
                return null;

            return new OrderDetails(order.Id, order.Timestamp);
        }
    }
}
