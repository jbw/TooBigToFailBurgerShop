
namespace TooBigToFailBurgerShop.Ordering.State
{
    using Automatonymous;
    using GreenPipes;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;
    using TooBigToFailBurgerShop.Ordering.Contracts;
    using TooBigToFailBurgerShop.Ordering.Domain.Events;

    public class BurgerOrderStateMachine : MassTransitStateMachine<BurgerOrderStateInstance>
    {
        private readonly ILogger<BurgerOrderStateMachine> _logger;
        public Event<OrderCreated>? NewOrder { get; set; }
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
            Event(() => NewOrder, x =>
            {
                x.CorrelateById(m => m.Message.AggregateId);
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
                When(NewOrder)
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

        }

        private async Task SendOrderForProcessing(BehaviorContext<BurgerOrderStateInstance, OrderCreated> context)
        {
            // get order and upstate status and dispatch event
            // https://blog.jonathanoliver.com/cqrs-sagas-with-event-sourcing-part-ii-of-ii/
            _logger.LogInformation("Initiating order for processing: {0}", context.Data.AggregateId);

            var order = CreateProcessBurgerOrder(context.Data);

            var payload = context.GetPayload<ConsumeContext>();

            var queueName = $"queue:{typeof(ProcessBurgerOrder).Name}";

            var endpoint = await payload.GetSendEndpoint(new Uri(queueName));

            await endpoint.Send(order).ConfigureAwait(false);

        }

        private void Initialize(BehaviorContext<BurgerOrderStateInstance, OrderCreated> context)
        {
            _logger.LogInformation("Initializing: {0}", context.Data.AggregateId);
           
            InitializeInstance(context.Instance, context.Data);
        }

        private static void InitializeInstance(BurgerOrderStateInstance instance, OrderCreated burgerOrderReceived)
        {
            instance.BurgerOrderId = burgerOrderReceived.AggregateId;
        }

        private static ProcessBurgerOrder CreateProcessBurgerOrder(OrderCreated burgerOrder)
        {
            return new ProcessBurgerOrder
            {
                CorrelationId = burgerOrder.AggregateId,
                OrderId = burgerOrder.AggregateId,
                OrderDate = burgerOrder.Timestamp,
            };
        }

        private void LogOrderReceived(BehaviorContext<BurgerOrderStateInstance, OrderCreated> context)
        {
            _logger.LogInformation("Order recieved: {0}", context.Data.AggregateId);
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
