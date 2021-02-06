using System;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork;
using TooBigToFailBurgerShop.Ordering.Domain.Events;

namespace TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel
{
    public class Order : AggregateRoot<Order, Guid>
    {

        public Order(Guid id) : base(id)
        {
            AddEvent(new OrderCreated(this));
        }

    }
}
