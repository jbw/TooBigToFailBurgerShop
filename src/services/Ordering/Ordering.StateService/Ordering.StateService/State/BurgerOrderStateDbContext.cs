using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TooBigToFailBurgerShop.Ordering.State;

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
}