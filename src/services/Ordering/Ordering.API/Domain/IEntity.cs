namespace TooBigToFailBurgerShop.Ordering.Domain
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
