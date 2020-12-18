using MediatR;
using System;

namespace TooBigToFailBurgerShop.Application.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand(string userId, Guid requestId)
        {
            UserId = userId;
            RequestId = requestId;
        }

        public Guid RequestId { get; set; }
        public string UserId { get; }
    }
}
