using MediatR;

namespace TooBigToFailBurgerShop.Application.Commands.Order
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand()
        {

        }
    }
}
