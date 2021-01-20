using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface BurgerOrderFaulted : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}