using MediatR;

namespace TooBigToFailBurgerShop.Application.Commands
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public CreateOrderCommand(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
