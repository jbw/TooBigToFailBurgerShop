using System;
using System.Threading;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IOrderIdRepository
    {
        Task CreateAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
