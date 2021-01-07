using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface CreateBurgerOrderReceived : CorrelatedBy<Guid> { }
}