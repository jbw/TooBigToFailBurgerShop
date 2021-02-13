namespace CreateOrder.Consumer.Application.Options
{
    class MongoConnectionSettings : ConnectionSettings
    {
        public string CollectionName { get; set; }
    }

}
