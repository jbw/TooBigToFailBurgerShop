using System;
using MassTransit;

namespace TooBigToFailBurgerShop.CreateOrder.Contracts
{
    public record CreateBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}