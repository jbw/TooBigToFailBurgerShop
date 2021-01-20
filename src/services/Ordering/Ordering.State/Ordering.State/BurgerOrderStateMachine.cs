
namespace TooBigToFailBurgerShop.Ordering.State
{
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using TooBigToFailBurgerShop.Ordering.Contracts;

    public class BurgerOrderStateMachine : MassTransitStateMachine<BurgerOrderStateInstance>
    {
        private readonly ILogger<BurgerOrderStateMachine> _logger;
        public Event<BurgerOrderReceived>? BurgerOrderReceived { get; set; }
        public Event<BurgerOrderProcessed>? BurgerOrderProcessed { get; set; }
        public Event<BurgerOrderFaulted>? BurgerOrderFaulted { get; set; }

        public State? WaitingForProcessing { get; private set; }
        public State? Faulted { get; private set; }
        public State? Processed { get; private set; }


        public BurgerOrderStateMachine(ILogger<BurgerOrderStateMachine> logger)
        {
            _logger = logger;

            InstanceState(x => x.CurrentState);

            // Maps CorrelationId with this state machine
            Event(() => BurgerOrderReceived, x =>
            {
                x.CorrelateById(m => m.Message.OrderId);
            });

            Event(() => BurgerOrderProcessed, x =>
            {
                x.CorrelateById(m => m.Message.OrderId);
            });

            Event(() => BurgerOrderFaulted, x =>
            {
                x.CorrelateById(m => m.Message.OrderId);
            });


            Initially(
                When(BurgerOrderReceived)
                    .Then(Initialize)
                    .Then(LogOrderReceived)
                    .ThenAsync(SendOrderForProcessing)
                    .TransitionTo(WaitingForProcessing));

            During(WaitingForProcessing,
                When(BurgerOrderProcessed)
                    .Then(LogOrderProcessed)
                    .TransitionTo(Processed)
                    .Finalize(),
                When(BurgerOrderFaulted)
                    .Then(LogOrderFaulted)
                    .TransitionTo(Faulted));


            SetCompletedWhenFinalized();

        }

        private async Task SendOrderForProcessing(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            _logger.LogInformation("Initiating order for processing: {0}", context.Data.OrderId);

            var order = CreateProcessBurgerOrder(context.Data);

            var payload = context.GetPayload<ConsumeContext>();

            var queueName = $"queue:{typeof(ProcessBurgerOrder).Name}";

            var endpoint = await payload.GetSendEndpoint(new Uri(queueName));

            await endpoint.Send(order).ConfigureAwait(false);

        }

        private void Initialize(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            _logger.LogInformation("Initializing: {0}", context.Data.OrderId);

            InitializeInstance(context.Instance, context.Data);
        }

        private static void InitializeInstance(BurgerOrderStateInstance instance, BurgerOrderReceived burgerOrderReceived)
        {
            instance.CorrelationId = burgerOrderReceived.CorrelationId;
            instance.BurgerOrderId = burgerOrderReceived.OrderId;
        }

        private static ProcessBurgerOrder CreateProcessBurgerOrder(BurgerOrderReceived burgerOrder)
        {
            return new ProcessBurgerOrder
            {
                CorrelationId = burgerOrder.CorrelationId,
                OrderId = burgerOrder.OrderId,
                OrderDate = burgerOrder.OrderDate,
            };
        }

        private void LogOrderReceived(BehaviorContext<BurgerOrderStateInstance, BurgerOrderReceived> context)
        {
            _logger.LogInformation("Order recieved: {0}", context.Data.OrderId);
        }

        private void LogOrderFaulted(BehaviorContext<BurgerOrderStateInstance, BurgerOrderFaulted> context)
        {
            _logger.LogInformation("Order faulted: {0}", context.Data.OrderId);
        }

        private void LogOrderProcessed(BehaviorContext<BurgerOrderStateInstance, BurgerOrderProcessed> context)
        {
            _logger.LogInformation("Order processed: {0}", context.Data.OrderId);
        }
    }
}
