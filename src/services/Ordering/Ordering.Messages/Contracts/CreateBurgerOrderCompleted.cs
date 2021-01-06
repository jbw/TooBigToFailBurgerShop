using MassTransit;
using System;

namespace TooBigToFailBurgerShop.CreateOrder.Contracts
{
    public interface CreateBurgerOrderCompleted : CorrelatedBy<Guid> { }
}