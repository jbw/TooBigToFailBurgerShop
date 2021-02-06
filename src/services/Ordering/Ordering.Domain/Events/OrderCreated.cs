using System;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;

namespace TooBigToFailBurgerShop.Ordering.Domain.Events
{
    public class OrderCreated : DomainEvent<Order, Guid>
    {
        // Need this for deserialisation
        protected OrderCreated() { }

        public OrderCreated(Order order) : base(order)
        {

        }
    }
}
