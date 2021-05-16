using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public record SubmitBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; init; }
        public Guid CustomerId { get; init; }
        public DateTime OrderDate { get; init; }
        public Guid CorrelationId { get; init; }
    }
}