using System.Collections.Generic;
using System.Collections.Immutable;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork
{
    public abstract class AggregateRoot<TType, TKey> : Entity<TKey>, IAggregateRoot<TKey> where TType : class, IAggregateRoot<TKey>
    {
        public long Version { get; private set; }

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();

        protected AggregateRoot(TKey id) : base(id)
        {

        }

        protected void AddEvent(IDomainEvent<TKey> domainEvent)
        {
            _events.Enqueue(domainEvent);

            Version++;
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
