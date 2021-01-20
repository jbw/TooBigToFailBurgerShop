using MediatR;
using System;

namespace TooBigToFailBurgerShop.Application.Commands.Order
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand( )
        {
            
        } 
    }
}
