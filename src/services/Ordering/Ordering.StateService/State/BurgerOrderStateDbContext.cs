using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Collections.Generic;

namespace Ordering.StateService
{

    public class BurgerOrderStateDbContext : SagaDbContext
    {

        public BurgerOrderStateDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new BurgerOrderStateStateMap();
            }
        }
    }

    public class BurgerShopContextFactory : IDesignTimeDbContextFactory<BurgerOrderStateDbContext>
    {

        public BurgerOrderStateDbContext CreateDbContext(string[] args)
        {

            var connectionSting = "host=localhost;database=burgerOrdersState;user id=burger;password=burger";

            var optionsBuilder = new DbContextOptionsBuilder<BurgerOrderStateDbContext>();

            optionsBuilder.UseNpgsql(connectionSting);

            return new BurgerOrderStateDbContext(optionsBuilder.Options);
        }
    }
}