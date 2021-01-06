
namespace TooBigToFailBurgerShop.Application.State
{
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using TooBigToFailBurgerShop.CreateOrder.Contracts;


    public class BurgerOrderStateMachine : MassTransitStateMachine<BurgerOrderStateInstance>
    {
        private ILogger<BurgerOrderStateMachine> _logger;
        public Event<CreateBurgerOrderReceived> EventCreateBurgerOrderReceived { get; set; }
        public Event<CreateBurgerOrderCompleted> EventCreateBurgerOrderCompleted { get; set; }
        public Event<CreateBurgerOrderFailed> EventCreateBurgerOrderFailed { get; set; }
        public State BurgerOrderReceived { get; private set; }
        public State BurgerOrdered { get; private set; }
        public State BurgerOrderFailed { get; private set; }

        public BurgerOrderStateMachine(ILogger<BurgerOrderStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);

            // Maps CorrelationId with this state machine
            Event(() => EventCreateBurgerOrderReceived, x =>
            {
                x.CorrelateById(m => m.Message.CorrelationId);
            });

            Event(() => EventCreateBurgerOrderCompleted, x =>
            {
                x.CorrelateById(m => m.Message.CorrelationId);
            });

            Event(() => EventCreateBurgerOrderFailed, x =>
            {
                x.CorrelateById(m => m.Message.CorrelationId);
            });

            Initially(
                When(EventCreateBurgerOrderReceived)
                    .Then(Initialize)
                    .ThenAsync(InitiateCreateBurgerOrder)
                    .RequestStarted()
                    .TransitionTo(BurgerOrderReceived));

            During(BurgerOrderReceived,
                When(EventCreateBurgerOrderCompleted)
                    .Then(Order)
                    .TransitionTo(BurgerOrdered),
                When(EventCreateBurgerOrderFailed)
                    .Then(OrderFailed)
                    .TransitionTo(BurgerOrderFailed));

        }


        private void OrderFailed(BehaviorContext<BurgerOrderStateInstance, CreateBurgerOrderFailed> context)
        {
            _logger.LogInformation("Order failed: {0}", context.Data.CorrelationId);
        }

        private void Order(BehaviorContext<BurgerOrderStateInstance, CreateBurgerOrderCompleted> context)
        {
            _logger.LogInformation("Order created: {0}", context.Data.CorrelationId);
        }

        private async Task InitiateCreateBurgerOrder(BehaviorContext<BurgerOrderStateInstance, CreateBurgerOrderReceived> context)
        {
            var order = CreateProcessBurgerOrder(context.Data);

            var payload = context.GetPayload<ConsumeContext>();

            var endpoint = await payload.GetSendEndpoint(new Uri($"queue:{typeof(CreateBurgerOrder).Name}"));

            await endpoint.Send(order).ConfigureAwait(false);

            _logger.LogInformation("Creating burger order: {0}", context.Data.CorrelationId);
        }

        private void Initialize(BehaviorContext<BurgerOrderStateInstance, CreateBurgerOrderReceived> context)
        {
            _logger.LogInformation("Initializing: {0}", context.Data.CorrelationId);

            InitializeInstance(context.Instance, context.Data);
        }

        private void InitializeInstance(BurgerOrderStateInstance instance, CreateBurgerOrderReceived burgerOrderReceived)
        {
            instance.CorrelationId = burgerOrderReceived.CorrelationId;
        }

        private CreateBurgerOrder CreateProcessBurgerOrder(CreateBurgerOrderReceived burgerOrderReceived)
        {
            return new CreateBurgerOrder { CorrelationId = burgerOrderReceived.CorrelationId };
        }
    }
}
