namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        protected Entity(TKey id) => Id = id;

        public TKey Id { get; set; }
    }
}
