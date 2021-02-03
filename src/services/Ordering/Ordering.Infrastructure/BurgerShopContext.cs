using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using TooBigToFailBurgerShop.Ordering.Infrastructure.EntityConfigurations;

namespace TooBigToFailBurgerShop.Ordering.Infrastructure
{
    public class BurgerShopContext: DbContext
    {
        public BurgerShopContext() { }

        public BurgerShopContext(DbContextOptions<BurgerShopContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BurgerOrderEntityTypeConfiguration());
        }
    }

    public class BurgerShopContextFactory : IDesignTimeDbContextFactory<BurgerShopContext>
    {

        public BurgerShopContext CreateDbContext(string[] args)
        {
  
            var connectionSting = "host=localhost;database=burgers;user id=burger;password=burger";

            var optionsBuilder = new DbContextOptionsBuilder<BurgerShopContext>();
          
            optionsBuilder.UseNpgsql(connectionSting);

            return new BurgerShopContext(optionsBuilder.Options);
        }
    }

}
