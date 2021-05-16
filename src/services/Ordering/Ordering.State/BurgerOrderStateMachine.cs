
namespace TooBigToFailBurgerShop.Ordering.State
{
    using Automatonymous;
    using MassTransit;
    using Microsoft.Extensions.Logging;
    using TooBigToFailBurgerShop.Ordering.Contracts;
    using TooBigToFailBurgerShop.Ordering.Domain.Events;

    public class BurgerOrderStateMachine : MassTransitStateMachine<BurgerOrderStateInstance>
    {
        private readonly ILogger<BurgerOrderStateMachine> _logger;

        public Event<SubmitBurgerOrder>? SubmitOrder { get; set; }
        public Event<OrderCreated>? BurgerOrderCreated { get; set; }
        public Event<BurgerOrderProcessed>? BurgerOrderProcessed { get; set; }
        public Event<BurgerOrderFaulted>? BurgerOrderFaulted { get; set; }

        public State? OrderSubmitted { get; private set; }
        public State? WaitingForProcessing { get; private set; }
        public State? OrderFaulted { get; private set; }
        public State? OrderProcessed { get; private set; }

        public BurgerOrderStateMachine(ILogger<BurgerOrderStateMachine> logger)
        {

            _logger = logger;

            InstanceState(x => x.CurrentState);

            // Maps CorrelationId with this state machine
            Event(() => SubmitOrder, x =>
            {
                x.CorrelateById(m => m.Message.OrderId);
            });

            Event(() => BurgerOrderCreated, x =>
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
                When(SubmitOrder)
                    .Then(Initialize)
                    // Start create order process
                    .PublishAsync(context => context.Init<CreateBurgerOrder>(new
                    {
                        context.Data.OrderDate,
                        context.Data.OrderId,
                        context.Data.CustomerId,
                        context.Data.CorrelationId
                    }))
                    .TransitionTo(OrderSubmitted));

            During(OrderSubmitted,
                // Hooking into domain events created in CreateBurgerOrder event
                When(BurgerOrderCreated)
                    .Then(LogOrderReceived)
                    .PublishAsync(context => context.Init<ProcessBurgerOrder>(CreateProcessBurgerOrder(context.Data)))
                    .TransitionTo(WaitingForProcessing));

            During(WaitingForProcessing,
                When(BurgerOrderProcessed)
                    .Then(LogOrderProcessed)
                    .TransitionTo(OrderProcessed)
                    .Finalize(),
                When(BurgerOrderFaulted)
                    .Then(LogOrderFaulted)
                    .TransitionTo(OrderFaulted));

            SetCompletedWhenFinalized();

        }

        private void Initialize(BehaviorContext<BurgerOrderStateInstance, SubmitBurgerOrder> context)
        {
            _logger.LogInformation("Initializing: {0}", context.Data.OrderId);

            InitializeInstance(context.Instance, context.Data);
        }

        private static void InitializeInstance(BurgerOrderStateInstance instance, SubmitBurgerOrder burgerOrderReceived)
        {
            instance.BurgerOrderId = burgerOrderReceived.OrderId;
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
