using Microsoft.Extensions.Logging;

namespace TooBigToFailBurgerShop.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        public RequestManager(BurgerShopContext context, ILogger<RequestManager> logger)
        {

        }
    }
}
