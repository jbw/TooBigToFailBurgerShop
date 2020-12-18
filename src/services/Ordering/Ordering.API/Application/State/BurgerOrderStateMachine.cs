
namespace TooBigToFailBurgerShop.Application.State
{
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using TooBigToFailBurgerShop.Ordering.Messages;

    public class BurgerOrderStateMachine : MassTransitStateMachine<BurgerOrderStateInstance>
    {
        private ILogger<BurgerOrderStateMachine> _logger;

        public Event<BurgerOrderReceived> EventBurgerOrderReceived { get; set; }
        public Event<BurgerOrderCompleted> EventBurgerOrderCompleted { get; set; }
        public Event<BurgerOrderFailed> EventBurgerOrderFailed { get; set; }

        public State Received { get; private set; }
        public State Ordered { get; private set; }
        public State Failed { get; private set; }

        public BurgerOrderStateMachine(ILogger<BurgerOrderStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);

            // Map CorrelationId  with this state machine
            Event(() => EventBurgerOrderReceived, x =>
            {
                x.CorrelateById(m => m.Message.CorrelationId);
            });

            Initially(
                When(EventBurgerOrderReceived)
                    .Then(Initialize)
                    .ThenAsync(InitiateOrderProcessing)
                    .TransitionTo(Received),
                When(EventBurgerOrderFailed)
                    .Then(OrderFailed)
                    .TransitionTo(Failed));

            During(Received,
                When(EventBurgerOrderCompleted)
                    .Then(Order)
                    .TransitionTo(Ordered));

        }

        private void OrderFailed(BehaviorContext<BurgerOrderStateInstance, BurgerOrderFailed> context)
        {
            _logger.LogInformation("Order failed: {0}", context.Data.CorrelationId);
        }

        private void Order(BehaviorContext<BurgerOrderStateInstance, BurgerOrderCompleted> context)
        {
            _logger.LogInformation("Order: {0}", context.Data.CorrelationId);
        }

        private async Task InitiateOrderProcessing(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            var order = CreateProcessBurgerOrder(context.Data);

            await context.GetPayload<ConsumeContext>().Send(order);

            _logger.LogInformation("Processing: {0}", context.Data.CorrelationId);

        }

        private void Initialize(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            _logger.LogInformation("Initializing: {0}", context.Data.CorrelationId);

            InitializeInstance(context.Instance, context.Data);
        }
        private void InitializeInstance(BurgerOrderStateInstance instance, BurgerOrderReceived burgerOrderReceived)
        {
            instance.CorrelationId = burgerOrderReceived.CorrelationId;
        }

        private ProcessBurgerOrder CreateProcessBurgerOrder(BurgerOrderReceived burgerOrderReceived)
        {
            return new Process { CorrelationId = burgerOrderReceived.CorrelationId };
        }

        class Process : ProcessBurgerOrder
        {
            public Guid CorrelationId { get; set; }
        }
    }

}
