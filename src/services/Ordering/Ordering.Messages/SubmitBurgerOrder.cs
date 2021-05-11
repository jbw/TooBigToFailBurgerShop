using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public record SubmitBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CorrelationId { get; set; }
    }
}