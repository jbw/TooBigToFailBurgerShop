
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
            //_publishEndpoint = publishEndpoint;

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

            // TODO The NewOrder event is not handled during the WaitingForProcessing state for the BurgerOrderStateMachine state machine

            // OrderSubmitted
            // OrderCreated
            // OrderProcessed

            Initially(
                When(SubmitOrder)
                    .Then(Initialize)
                    // Start create order process 
                    .PublishAsync(context => context.Init<CreateBurgerOrder>(new
                    {
                        OrderDate = context.Data.OrderDate,
                        OrderId = context.Data.OrderId,
                        CorrelationId = context.Data.CorrelationId
                    }))
                    .TransitionTo(OrderSubmitted));

            During(OrderSubmitted,
                // Hooking into domain events created in CreateBurgerOrder event
                When(BurgerOrderCreated) 
                    .Then(LogOrderReceived)
                    .PublishAsync(context => context.Init<ProcessBurgerOrder>(CreateProcessBurgerOrder(context.Data)))
                    //.ThenAsync(SendOrderForProcessing)
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

        private async Task CreateOrder(BehaviorContext<BurgerOrderStateInstance, SubmitBurgerOrder> context)
        {

            _logger.LogInformation("CreateOrder : {0}", context.Data.OrderId);

            var orderData = context.Data;

            var message = new
            {
                OrderDate = orderData.OrderDate,
                OrderId = orderData.OrderId,
                CorrelationId = orderData.CorrelationId
            };

            var payload = context.GetPayload<ConsumeContext>();
            var queueName = $"queue:{typeof(CreateBurgerOrder).Name}";
            var endpoint = await payload.GetSendEndpoint(new Uri(queueName));
            await endpoint.Send<CreateBurgerOrder>(message).ConfigureAwait(false);

        }

        private async Task SendOrderForProcessing(BehaviorContext<BurgerOrderStateInstance, OrderCreated> context)
        {

            _logger.LogInformation("Initiating order for processing: {0}", context.Data.AggregateId);

            var order = CreateProcessBurgerOrder(context.Data);

            var payload = context.GetPayload<ConsumeContext>();

            var queueName = $"queue:{typeof(ProcessBurgerOrder).Name}";

            var endpoint = await payload.GetSendEndpoint(new Uri(queueName));

            await endpoint.Send(order).ConfigureAwait(false);

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
