
namespace TooBigToFailBurgerShop.Application.State
{
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using System;
    using System.Threading.Tasks;
    using TooBigToFailBurgerShop.Ordering.Messages;

    public class BurgerOrderStateMachine : MassTransitStateMachine<BurgerOrderStateInstance>
    {
        public Event<BurgerOrderReceived> EventBurgerOrderReceived { get; set; }
        public Event<BurgerOrderCompleted> EventBurgerOrderCompleted { get; set; }
        public Event<BurgerOrderFailed> EventBurgerOrderFailed { get; set; }

        public State Received { get; private set; }
        public State Ordered { get; private set; }
        public State Failed { get; private set; }

        public BurgerOrderStateMachine()
        {

            InstanceState(x => x.CurrentState);

            Event(() => EventBurgerOrderReceived, x =>
            {
                x.CorrelateById(m => m.Message.CorrelationId);

            });

            Initially(
                When(EventBurgerOrderReceived)
                    .Then(Initialize)
                    .ThenAsync(InitiateOrderProcessing)
                    .TransitionTo(Received));

            During(Received,
                When(EventBurgerOrderCompleted)
                    .Then(Order)
                    .TransitionTo(Ordered));

        }

        private async Task InitiateOrderProcessing(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            await context.GetPayload<ConsumeContext>().Send(new { });
        }

        private void Initialize(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            throw new NotImplementedException();
        }
        private void Order(BehaviorContext<BurgerOrderStateInstance, BurgerOrderCompleted> context)
        {
            throw new NotImplementedException();
        }
    }

}
