using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface OrderSubmitted : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}