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
    public class CreateBurgerOrderConsumer : IConsumer<CreateBurgerOrder>
    {
        private readonly ILogger<CreateBurgerOrderConsumer> _logger;

        public CreateBurgerOrderConsumer(ILogger<CreateBurgerOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<CreateBurgerOrder> context)
        {
            _logger.LogInformation($"CreateBurgerOrderConsumer {context.Message.CorrelationId}");
            // Use unique ID and decouple tracking ID from any other IDs (e.g. Order Id) from the routing slip.
            // E.g we might execute a routing slip multiple times so we would want a new ID to track with. 
            var trackingId = NewId.NextGuid();
            var routingSlip = CreateRoutingSlip(context, trackingId);

            await context.Execute(routingSlip).ConfigureAwait(false);

        }

        private static RoutingSlip CreateRoutingSlip(ConsumeContext<CreateBurgerOrder> context, Guid trackingId)
        {
            var builder = new RoutingSlipBuilder(trackingId);

            builder.AddVariable("CorrelationId", context.CorrelationId);

            var queueName = $"{typeof(CreateBurgerOrderActivity).Name.Replace("Activity", "")}_execute";
            var activityName = "CreateBurgerOrderActivity";
            var executeAddress = new Uri($"queue:{queueName}");

            builder.AddActivity(activityName, executeAddress);

            builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Completed, x => x.Send<CreateBurgerOrderCompleted>(new { context.Message.CorrelationId }));

            return builder.Build();

        }
    }
}
