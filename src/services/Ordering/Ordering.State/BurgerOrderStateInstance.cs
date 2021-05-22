using Automatonymous;
using System;

namespace TooBigToFailBurgerShop.Ordering.State
{
    public class BurgerOrderStateInstance : SagaStateMachineInstance
    {
        public BurgerOrderStateInstance() { }

        public Guid BurgerOrderId { get; set; }
        public string? CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
