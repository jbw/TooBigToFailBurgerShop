using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public record BurgerOrderProcessed : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}