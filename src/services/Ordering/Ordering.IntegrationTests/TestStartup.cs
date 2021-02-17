using Microsoft.Extensions.Configuration;
using MassTransit;

namespace Ordering.IntegrationTests.Features.Order
{
    public class TestStartup : TooBigToFailBurgerShop.Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }

        public override void ConfigureMassTransit(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddMassTransit(cfg => cfg.UsingInMemory());
        }
    }
}
