using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface CreateBurgerOrderFailed : CorrelatedBy<Guid> { }
}