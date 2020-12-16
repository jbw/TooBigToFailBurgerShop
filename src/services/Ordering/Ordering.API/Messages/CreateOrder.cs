using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Application.Messages
{
    public interface CreateOrder : CorrelatedBy<Guid>
    {
        public DateTime? OrderDate { get; set; }

        public string? UserId { get; set; }
    }
}
