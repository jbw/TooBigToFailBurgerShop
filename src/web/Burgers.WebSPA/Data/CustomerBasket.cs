using System.Collections.Generic;

namespace Burgers.WebSPA.Data
{
    public class CustomerBasket
    {
        public string CustomerId { get; private set; }

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public CustomerBasket(string customerId)
        {
            CustomerId = customerId;
        }

    }
}
