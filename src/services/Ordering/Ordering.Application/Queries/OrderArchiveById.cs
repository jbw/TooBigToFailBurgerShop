using MediatR;
using System;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;

namespace TooBigToFailBurgerShop.Ordering.Application.Queries
{
    public class OrderArchiveById : IRequest<OrderArchiveItem>
    {
        public Guid Id { get; }

        public OrderArchiveById(Guid id) => Id = id;
    }
}
