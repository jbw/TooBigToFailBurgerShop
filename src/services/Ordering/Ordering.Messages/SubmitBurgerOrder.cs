using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Messages
{
    public interface SubmitBurgerOrder : CorrelatedBy<Guid>
    {
        public DateTime? OrderDate { get; set; }

        public string? UserId { get; set; }
    }

    public record ProcessBurgerOrder : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
    public interface BurgerOrderCompleted : CorrelatedBy<Guid> { }
    public interface BurgerOrderFailed : CorrelatedBy<Guid> { }
    public interface BurgerOrderReceived : CorrelatedBy<Guid> { }
}
