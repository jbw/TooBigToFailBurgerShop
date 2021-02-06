using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject.Extensions;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject
{

    public abstract class ValueObject
    {
        protected ValueObject() { }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            var t = GetType();
            if (t != obj.GetType())
                return false;

            return this.ValueObjectAreEqual(obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            int multiplier = 59;

            int hashCode = hash;

            var props = GetType().GetProperties();
            foreach (var prop in props)
            {
                var value = prop.GetValue(this, null);
                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            var fields = GetType().GetFields();
            foreach (var field in fields)
            {
                object? value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }

        public static bool operator ==(ValueObject a, ValueObject b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b)
        {
            return a != b;
        }

    }
}
