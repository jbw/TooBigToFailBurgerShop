using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface BurgerOrderFaulted : CorrelatedBy<Guid> { }
}