using Automatonymous;
using System;

namespace TooBigToFailBurgerShop
{
    public class BurgerOrderStateInstance : SagaStateMachineInstance
    {
        public Guid BurgerOrderId { get; set; }
        public string? CurrentState { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
