using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface CreateBurgerOrderCompleted : CorrelatedBy<Guid> { }
}