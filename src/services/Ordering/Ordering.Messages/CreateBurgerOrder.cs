using System;
using MassTransit;

namespace TooBigToFailBurgerShop.Ordering.Contracts
{
    public interface CreateBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
    }
}