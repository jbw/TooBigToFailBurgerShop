using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateOrder.Consumer.Application.Options
{
    class ConnectionSettings
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int Port { get; set; }
    }

    class MongoConnectionSettings : ConnectionSettings
    {
        public string CollectionName { get; set; }
    }

    class BurgerShopSettings
    {
        public ConnectionSettings Connection { get; set; }
    }

    class BurgerShopEventsSettings
    {
        public ConnectionSettings Connection { get; set; }
    }

    class OrderIdRepositorySettings
    {
        public MongoConnectionSettings Connection { get; set; }
    }

}
