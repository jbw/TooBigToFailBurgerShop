using Microsoft.EntityFrameworkCore;

namespace TooBigToFailBurgerShop.Infrastructure
{
    public class BurgerShopContext: DbContext
    {
        public BurgerShopContext() { }

        public BurgerShopContext(DbContextOptions<BurgerShopContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
