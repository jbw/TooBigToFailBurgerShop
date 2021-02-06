using System;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork
{
    public abstract class DomainEvent<TType, TKey> : IDomainEvent<TKey> where TType : IAggregateRoot<TKey>
    {
        public long AggregateVersion { get; init; }

        public TKey AggregateId { get; init; }

        public DateTime Timestamp { get; init; }

        // Need this for deserialisation
        protected DomainEvent() { }

        public DomainEvent(TType aggregateRoot)
        {
            AggregateId = aggregateRoot.Id;
            AggregateVersion = aggregateRoot.Version;
            Timestamp = DateTime.UtcNow;
        }
    }
}
