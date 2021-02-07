using MediatR;
using System;
using TooBigToFailBurgerShop.Application.Queries.Models;

namespace TooBigToFailBurgerShop.Application.Queries
{ 
    public class OrderById : IRequest<OrderDetails>
    {
        public Guid Id { get; }

        public OrderById(Guid id) => Id = id;

    }
}
