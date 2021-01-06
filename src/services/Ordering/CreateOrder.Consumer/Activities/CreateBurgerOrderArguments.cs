using System;

namespace TooBigToFailBurgerShop.Ordering.Activities
{
    public interface CreateBurgerOrderArguments
    {
        Guid CorrelationId { get; }
    }
}