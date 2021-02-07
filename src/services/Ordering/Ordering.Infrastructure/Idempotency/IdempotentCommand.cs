using MediatR;
using System;

namespace TooBigToFailBurgerShop.Ordering.Infrastructure.Idempotency
{
    public class IdempotentCommand<T, R> : IRequest<R> where T : IRequest<R>
    {
        public T Command { get; }
        public Guid Id { get; }

        public IdempotentCommand(T command, Guid id)
        {
            Command = command;
            Id = id;
        }
    }
}
