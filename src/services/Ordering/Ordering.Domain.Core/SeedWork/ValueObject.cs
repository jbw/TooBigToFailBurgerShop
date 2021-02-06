namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork
{
    public abstract class ValueObject 
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return a.Equals(b);
        }
    }
}
