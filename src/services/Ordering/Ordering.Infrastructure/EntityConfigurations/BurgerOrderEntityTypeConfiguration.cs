using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TooBigToFailBurgerShop.Ordering.Domain.AggregatesModel;

namespace TooBigToFailBurgerShop.Ordering.Infrastructure.EntityConfigurations
{
    public class BurgerOrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            // we should ignore domain events
            builder.Ignore(b => b.Events);

        }
    }
}
