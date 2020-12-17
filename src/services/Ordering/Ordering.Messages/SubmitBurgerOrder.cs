using MassTransit;
using System;

namespace TooBigToFailBurgerShop.Ordering.Messages
{
    public interface SubmitBurgerOrder : CorrelatedBy<Guid>
    {
        public DateTime? OrderDate { get; set; }

        public string? UserId { get; set; }
    }

    public interface ProcessBurgerOrder : CorrelatedBy<Guid> { }
    public interface BurgerOrderCompleted : CorrelatedBy<Guid> { }
    public interface BurgerOrderFailed : CorrelatedBy<Guid> { }
    public interface BurgerOrderReceived : CorrelatedBy<Guid> { }
}
