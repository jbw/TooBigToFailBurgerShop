using MediatR;
using System;

namespace TooBigToFailBurgerShop.Application.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand(Guid requestId)
        {
            RequestId = requestId;
        }

        public Guid RequestId { get; set; }
    }
}
