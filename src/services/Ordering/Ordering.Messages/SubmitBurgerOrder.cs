using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public record SubmitBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}