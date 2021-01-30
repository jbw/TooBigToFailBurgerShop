using System;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public class OrderCreated : DomainEvent<Order, Guid>
    {
        public OrderCreated(Order order) : base(order)
        {

        }
    }
}
