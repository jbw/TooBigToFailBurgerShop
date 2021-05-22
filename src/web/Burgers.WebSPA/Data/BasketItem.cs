namespace Burgers.WebSPA.Data
{
    public class BasketItem
    {
        public BasketItem(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
    }
}
