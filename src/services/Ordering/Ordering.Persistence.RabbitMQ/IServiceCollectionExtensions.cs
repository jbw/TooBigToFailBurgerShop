
using Microsoft.Extensions.DependencyInjection;

namespace TooBigToFailBurgerShop.Ordering.Persistence.MartenDb
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqInfrastructure(this IServiceCollection services)
        {
            return services;
        }
    }
}
