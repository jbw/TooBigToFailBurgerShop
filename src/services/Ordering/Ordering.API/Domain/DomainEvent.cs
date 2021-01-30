using System;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public abstract class DomainEvent<TType, TKey> : IDomainEvent<TKey> where TType : IAggregateRoot<TKey>
    {
        public long AggregateVersion { get; }

        public TKey AggregateId { get; }

        public DateTime Timestamp { get; }

        public DomainEvent(TType aggregateRoot)
        {
            AggregateId = aggregateRoot.Id;
            AggregateVersion = aggregateRoot.Version;
            Timestamp = DateTime.Now;
        }
    }
}
