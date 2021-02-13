using System;

namespace TooBigToFailBurgerShop.Ordering.Application.Queries.Models
{
    public record OrderId
    {
        public Guid Id { get; set; }

        public OrderId(Guid id) => Id = id;
    }
}
