using MediatR;
using System.Collections.Generic;
using TooBigToFailBurgerShop.Ordering.Application.Queries.Models;

namespace TooBigToFailBurgerShop.Ordering.Application.Queries
{
    public class OrdersArchive : IRequest<IEnumerable<OrderArchiveItem>> { }
}
