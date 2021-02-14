using System;

namespace TooBigToFailBurgerShop.Ordering.Application.Queries.Models
{
    public record OrderId
    {
        public OrderId(Guid id) => Id = id;

        public Guid Id { get; set; }
    }
}
