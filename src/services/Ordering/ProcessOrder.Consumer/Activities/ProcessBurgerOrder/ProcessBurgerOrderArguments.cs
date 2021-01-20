using System;

namespace TooBigToFailBurgerShop.Ordering.Activities
{
    public interface ProcessBurgerOrderArguments
    {
        Guid OrderId { get; }
    }
}