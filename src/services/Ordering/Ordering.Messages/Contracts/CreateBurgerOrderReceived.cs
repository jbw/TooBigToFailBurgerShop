using MassTransit;
using System;

namespace TooBigToFailBurgerShop.CreateOrder.Contracts
{
    public interface CreateBurgerOrderReceived : CorrelatedBy<Guid> { }
}