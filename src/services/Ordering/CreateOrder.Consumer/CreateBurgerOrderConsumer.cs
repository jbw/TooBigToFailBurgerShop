using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Core;
using System;
using System.Threading.Tasks;
using System.Transactions;
using TooBigToFailBurgerShop.Ordering.Contracts;
using TooBigToFailBurgerShop.Ordering.Domain;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;
using TooBigToFailBurgerShop.Ordering.Domain.Core;

namespace TooBigToFailBurgerShop.Ordering.CreateOrder.Consumer
{

    public class CreateBurgerOrderConsumer : IConsumer<CreateBurgerOrder>
    {
        private readonly ILogger<CreateBurgerOrderConsumer> _logger;
        private readonly IEventsService<Order, Guid> _orderEventsService;
        private readonly IOrderIdRepository _orderIdRepository;

        public CreateBurgerOrderConsumer(IOrderIdRepository orderIdRepository, IEventsService<Order, Guid> orderEventsService, ILogger<CreateBurgerOrderConsumer> logger)
        {
            _logger = logger;
            _orderEventsService = orderEventsService;
            _orderIdRepository = orderIdRepository;
        }

        public async Task Consume(ConsumeContext<CreateBurgerOrder> context)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
            {

                if (await _orderIdRepository.ExistsAsync(context.Message.OrderId))
                {
                    throw new ValidationException("Unable to create Order", new ValidationError(nameof(CreateBurgerOrder), $"Id '{context.Message.OrderId}' already exists"));
                }

                var orderAggregate = new Order(context.Message.OrderId, context.Message.CustomerId);

                // Here we are doing 3 things: 
                // * persisting the aggregate
                // * publishing domain events
                // * and persisting order id. 
                // This is across multiple technologies (postgres, mongo, rabbitmq).
                // We are concerned with data consistency.
                
                await _orderEventsService.PersistAsync(orderAggregate);
                await _orderIdRepository.CreateAsync(context.Message.OrderId);

                scope.Complete();
            }

        }
    }
}
