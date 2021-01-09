using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface BurgerOrderProcessed : CorrelatedBy<Guid> { }

}