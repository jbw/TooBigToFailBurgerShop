using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface SubmitBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid RequestId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}