using Microsoft.Extensions.Logging;
using TooBigToFailBurgerShop.Ordering.Infrastructure;

namespace TooBigToFailBurgerShop.Ordering.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        public RequestManager(BurgerShopContext context, ILogger<RequestManager> logger)
        {

        }
    }
}
