using System;
using System.Threading;
using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IOrderArchiveItemRepository
    {
        Task CreateAsync(Guid order, DateTime timestamp, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid orderId, CancellationToken cancellationToken = default);
    }
}
