using MassTransit;
using System;

namespace TooBigToFailBurgerShop.CreateOrder.Contracts
{
    public interface CreateBurgerOrderFailed : CorrelatedBy<Guid> { }
}