using System;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IOrdersRepository
    {
        Task CreateAsync(Guid orderId);
    }
}
