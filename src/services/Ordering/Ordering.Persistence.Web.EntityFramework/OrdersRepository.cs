using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Web.EntityFramework
{
    public class OrdersRepository : IOrdersRepository
    {
        public Task CreateAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }
    }
}
