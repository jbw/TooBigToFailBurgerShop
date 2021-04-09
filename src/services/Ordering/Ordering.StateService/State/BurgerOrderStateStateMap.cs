using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TooBigToFailBurgerShop.Ordering.State;

namespace Ordering.StateService
{
    public class BurgerOrderStateStateMap : SagaClassMap<BurgerOrderStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<BurgerOrderStateInstance> entity, ModelBuilder model)
        {

            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.BurgerOrderId);

        }
    }
}