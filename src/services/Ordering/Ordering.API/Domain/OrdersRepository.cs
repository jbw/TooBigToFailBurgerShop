using System;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public class OrdersRepository : IOrdersRepository
    {
        public Task CreateAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
