using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Core;
using System;
using System.Threading.Tasks;
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
        private readonly IOrdersRepository _orderRepository;

        public CreateBurgerOrderConsumer(IOrdersRepository ordersRepository, IEventsService<Order, Guid> orderEventsService, ILogger<CreateBurgerOrderConsumer> logger)
        {
            _logger = logger;
            _orderEventsService = orderEventsService;
            _orderRepository = ordersRepository;
        }

        public async Task Consume(ConsumeContext<CreateBurgerOrder> context)
        {

            _logger.LogInformation($"CreateBurgerOrderConsumer {context.MessageId}");

            if(await _orderRepository.ExistsAsync(context.Message.OrderId))
            {
                throw new ValidationException("Unable to create Order", new ValidationError(nameof(CreateBurgerOrder), $"Id '{context.Message.OrderId}' already exists"));
            }

            var orderAggregate = new Order(context.Message.OrderId);

            await _orderEventsService.PersistAsync(orderAggregate);
            await _orderRepository.CreateAsync(context.Message.OrderId);

        }
    }

}
