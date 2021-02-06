using MassTransit;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain;
using TooBigToFailBurgerShop.Ordering.Domain.Core;
using TooBigToFailBurgerShop.Ordering.Domain.Events;

namespace TooBigToFailBurgerShop.Ordering.CreateOrder.Consumer
{

    public class OrderArchiverConsumer : IConsumer<OrderCreated>
    {
        private readonly ILogger<CreateBurgerOrderConsumer> _logger;
        private readonly IOrdersRepository _orderRepository;

        public OrderArchiverConsumer(IOrdersRepository ordersRepository, ILogger<CreateBurgerOrderConsumer> logger)
        {
            _logger = logger;
            _orderRepository = ordersRepository;
        }

        public async Task Consume(ConsumeContext<OrderCreated> context)
        {
            var @event = context.Message;

            await _orderRepository.CreateAsync(@event.AggregateId, @event.Timestamp);
        }
    }

}
