using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core
{
    public interface IEventsService<TType, TKey>
    {
        Task PersistAsync(TType aggregateRoot);
    }
}
