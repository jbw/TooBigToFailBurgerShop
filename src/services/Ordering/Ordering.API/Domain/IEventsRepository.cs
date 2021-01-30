using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IEventsRepository<TType, TKey> where TType : class, IAggregateRoot<TKey>
    {
        Task AppendAsync(TType aggregateRoot);
        Task<TType> RehydrateAsync(TKey key);
    }
}
