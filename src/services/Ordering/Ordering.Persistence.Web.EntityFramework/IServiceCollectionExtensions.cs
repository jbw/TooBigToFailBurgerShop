
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TooBigToFailBurgerShop.Ordering.Domain;

namespace TooBigToFailBurgerShop.Ordering.Persistence.Web.EntityFramework
{
    public class AddEntityFrameworkOrderingPeristanceConfigurator
    {

    }
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFrameworkOrderRepository<TContext, TType>(this IServiceCollection services) where TContext : DbContext where TType : class
        {

            services.AddSingleton<IOrdersRepository, OrdersRepository<TContext, TType>>();

            return services;
        }
    }
}
