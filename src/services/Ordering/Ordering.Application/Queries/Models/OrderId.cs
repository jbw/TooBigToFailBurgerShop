using System;

namespace TooBigToFailBurgerShop.Ordering.Application.Queries.Models
{
    public record OrderId
    {
        public Guid Id { get; set; }

        private OrderId() { }

        public OrderId(Guid id) => Id = id;
    }
}
