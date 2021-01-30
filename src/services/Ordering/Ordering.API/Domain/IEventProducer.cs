using System.Threading.Tasks;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IEventProducer<in TType, in TKey> where TType : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TType aggregateKey);
    }
}
