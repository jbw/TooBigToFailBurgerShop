using Microsoft.Extensions.Configuration;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.IntegrationTests.Features.Order
{
    public class TestStartup : TooBigToFailBurgerShop.Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {

        }


        public override void ConfigureTracing(IServiceCollection services)
        {
            // ignore tracing 
        }
    }
}
