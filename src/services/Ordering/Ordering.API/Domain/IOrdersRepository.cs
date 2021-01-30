using System;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IOrdersRepository : IRepository
    {
        Task CreateAsync(Guid orderId);
    }
}
