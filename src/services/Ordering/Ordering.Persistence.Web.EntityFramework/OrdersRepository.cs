using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TooBigToFailBurgerShop.Ordering.Domain;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Web.EntityFramework
{
    public class OrdersRepository<TContext, TType> : IOrdersRepository where TContext : DbContext where TType : class
    {
        private TContext _context;

        public OrdersRepository(TContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Guid orderId)
        {
            await _context.AddAsync(new Order(orderId));
            await _context.SaveChangesAsync();
        }
    }
}
