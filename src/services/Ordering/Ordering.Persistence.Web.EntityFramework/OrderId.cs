using System;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Mongo
{
    internal class OrderId
    {
        public Guid Id { get; set; }
    }

    internal class OrderArchiveItem
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
