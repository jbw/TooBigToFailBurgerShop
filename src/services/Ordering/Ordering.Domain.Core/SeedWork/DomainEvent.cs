using System;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core
{
    public abstract class DomainEvent<TType, TKey> : IDomainEvent<TKey> where TType : IAggregateRoot<TKey>
    {
        public long AggregateVersion { get; set; }

        public TKey AggregateId { get; set; }

        public DateTime Timestamp { get; set; }

        public DomainEvent()
        {

        }

        public DomainEvent(TType aggregateRoot)
        {
            AggregateId = aggregateRoot.Id;
            AggregateVersion = aggregateRoot.Version;
            Timestamp = DateTime.Now;
        }
    }
}
