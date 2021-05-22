using System;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;
using TooBigToFailBurgerShop.Ordering.Domain.Events;

namespace TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel
{
    public class Order : AggregateRoot<Order, Guid>
    {
        public Guid CustomerId { get; set; }

        public Order(Guid orderId, Guid customerId) : base(orderId)
        {
            CustomerId = customerId;

            AddEvent(new OrderCreated(this));
        }

    }
}
