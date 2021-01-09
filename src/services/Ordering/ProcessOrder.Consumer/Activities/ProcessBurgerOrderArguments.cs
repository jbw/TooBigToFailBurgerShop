using System;

namespace TooBigToFailBurgerShop.Ordering.Activities
{
    public interface ProcessBurgerOrderArguments
    {
        Guid CorrelationId { get; }
    }
}