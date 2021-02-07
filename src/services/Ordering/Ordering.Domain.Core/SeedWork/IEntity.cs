namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork
{
    public interface IEntity<out TKey>
    {
        TKey Id { get; }
    }
}
