using Automatonymous;
using MassTransit;
using MassTransit.Courier;
using MassTransit.Courier.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Activities;
using TooBigToFailBurgerShop.Ordering.Contracts;

namespace TooBigToFailBurgerShop.ProcessOrder.Consumer
{

    public class ProcessBurgerOrderConsumer : IConsumer<ProcessBurgerOrder>
    {
        private readonly ILogger<ProcessBurgerOrderConsumer> _logger;

        public ProcessBurgerOrderConsumer(ILogger<ProcessBurgerOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ProcessBurgerOrder> context)
        {
            _logger.LogInformation($"ProcessBurgerOrderConsumer {context.Message.OrderId}");

            // Use unique ID and decouple tracking ID from any other IDs (Order Id) from the routing slip.
            // e.g we might execute a routing slip multiple times so we would want a new ID to track with. 
            var trackingId = NewId.NextGuid();

            // Create this routing slip in a consumer so we can use retry/fault handling
            var routingSlip = BuildRoutingSlip(context, trackingId);

            await context.Execute(routingSlip).ConfigureAwait(false);

        }

        private static RoutingSlip BuildRoutingSlip(ConsumeContext<ProcessBurgerOrder> context, Guid trackingId)
        {
            var builder = new RoutingSlipBuilder(trackingId);

            builder.AddVariable("OrderId", context.Message.OrderId);
            builder.AddVariable("CorrelationId", context.CorrelationId);

            // minor: abstract out this
            var activityName = typeof(ProcessBurgerOrderActivity).Name;
            var queueName = $"{activityName.Replace("Activity", string.Empty)}_execute";
            var executeAddress = new Uri($"queue:{queueName}");

            builder.AddActivity(activityName, executeAddress);

            // Completed
            builder
                .AddSubscription(
                    context.SourceAddress,
                    RoutingSlipEvents.Completed,
                    x => x.Send<BurgerOrderProcessed>(
                        new
                        {
                            context.Message.CorrelationId,
                            context.Message.OrderId,
                            context.Message.OrderDate
                        })
                    );

            // Faulted
            builder
                .AddSubscription(
                    context.SourceAddress,
                    RoutingSlipEvents.ActivityFaulted,
                    x => x.Send<BurgerOrderFaulted>(
                        new
                        {
                            context.Message.CorrelationId,
                            context.Message.OrderId,
                            context.Message.OrderDate
                        })
                    );

            return builder.Build();

        }
    }
}
