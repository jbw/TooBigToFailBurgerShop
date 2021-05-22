using System;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

namespace TooBigToFailBurgerShop.Ordering.Domain.Events
{
    public class OrderCreated : DomainEvent<Order, Guid>
    {
        // Need this for deserialisation
        protected OrderCreated() { }

        public Guid CustomerId { get; init; }

        public OrderCreated(Order order) : base(order)
        {
            CustomerId = order.CustomerId;
        }
    }
}
