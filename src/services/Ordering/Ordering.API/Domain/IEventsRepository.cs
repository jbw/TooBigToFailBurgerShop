using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IEventsRepository<TType, TKey> where TType : class, IAggregateRoot<TKey>, IRepository
    {
        Task AppendAsync(TType aggregateRoot);
        Task<TType> RehydrateAsync(TKey key);
    }
}
