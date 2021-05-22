using MediatR;
using System;

namespace TooBigToFailBurgerShop.Application.Commands.Order
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; set; }

        public Guid CustomerId { get; set; }
    }
}
