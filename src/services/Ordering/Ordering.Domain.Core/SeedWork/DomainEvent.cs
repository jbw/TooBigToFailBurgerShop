using System;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork
{
    public abstract class DomainEvent<TType, TKey> : IDomainEvent<TKey> where TType : IAggregateRoot<TKey>
    {
        public long AggregateVersion { get; private set; }

        public TKey AggregateId { get; private set; }

        public DateTime Timestamp { get; private set; }

        protected DomainEvent()
        {

        }

        public DomainEvent(TType aggregateRoot)
        {
            AggregateId = aggregateRoot.Id;
            AggregateVersion = aggregateRoot.Version;
            Timestamp = DateTime.UtcNow;
        }
    }
}
