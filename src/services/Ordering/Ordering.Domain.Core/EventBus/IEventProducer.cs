using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.EventBus
{
    public interface IEventProducer<in TType, in TKey> where TType : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TType aggregateKey);
    }
}
