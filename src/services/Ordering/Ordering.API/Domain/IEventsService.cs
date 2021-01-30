using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IEventsService<TType, TKey>
    {
        Task PersistAsync(TType aggregateRoot);
    }
}
