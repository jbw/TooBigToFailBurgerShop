namespace TooBigToFailBurgerShop.Ordering.Domain.Core
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
