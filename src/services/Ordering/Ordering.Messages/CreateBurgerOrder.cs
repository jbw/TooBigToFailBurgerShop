using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public record CreateBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }

    public record ProcessBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}