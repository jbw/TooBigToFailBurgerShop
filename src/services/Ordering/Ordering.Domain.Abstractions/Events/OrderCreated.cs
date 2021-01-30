using System;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using TooBigToFailBurgerShop.Ordering.Domain.Core;

namespace TooBigToFailBurgerShop.Ordering.Domain.Events
{
    public class OrderCreated : DomainEvent<Order, Guid>
    {
        public OrderCreated(Order order) : base(order)
        {

        }
    }
}
