﻿using System;
using TooBigToFailBurgerShop.Ordering.Domain.Core;

namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public class Order : AggregateRoot<Order, Guid>
    {
        public Order(Guid id) : base(id)
        {
            AddEvent(new OrderCreated(this));
        } 
    }
}