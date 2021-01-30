using System.Collections.Generic;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>
    {
        public long Version { get; }
        IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
        void ClearEvents();
    }
}
