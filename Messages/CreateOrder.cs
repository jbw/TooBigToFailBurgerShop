using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Application.Messages
{
    public record CreateOrder : CorrelatedBy<Guid>
    {
        public DateTime? OrderDate { get; }

        public string? UserId { get; init; }

        public Guid CorrelationId { get; init; }
    }
}
