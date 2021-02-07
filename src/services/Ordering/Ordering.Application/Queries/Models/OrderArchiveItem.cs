using System;

namespace TooBigToFailBurgerShop.Ordering.Application.Queries.Models
{ 
    public record OrderArchiveItem
    {
        private OrderArchiveItem() { }

        public OrderArchiveItem(Guid id, DateTime timestamp) => (Id, Timestamp) = (id, timestamp);

        public Guid Id { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
