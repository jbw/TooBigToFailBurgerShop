using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface BurgerOrderReceived : CorrelatedBy<Guid> { }

}