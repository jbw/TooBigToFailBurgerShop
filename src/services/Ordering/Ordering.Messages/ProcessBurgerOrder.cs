using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public record ProcessBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}