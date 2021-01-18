using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface BurgerOrderReceived : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid RequestId { get; set; }
        public DateTime OrderDate { get; set; }
    }

}