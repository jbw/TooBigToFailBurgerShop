using System;

namespace TooBigToFailBurgerShop.Application.Queries.Models
{ 
    public class OrderDetails
    {
        public OrderDetails(Guid id, DateTime timestamp) => (Id, Timestamp) = (id, timestamp);

        public Guid Id { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
