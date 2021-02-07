using System.Linq;
using System.Reflection;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject.Extensions
{
    internal static class ValueObjectEqualityExtensions
    {
        internal static bool ValueObjectAreEqual(this ValueObject a, object b)
        {
            return AllProperiesAreEqual(a, b) && AllFieldsAreEqual(a, b);
        }

        private static bool AllFieldsAreEqual(ValueObject a, object b)
        {
            return a.GetType().GetFields().All(f => FieldIsEqual(a, b, f));
        }

        private static bool AllProperiesAreEqual(ValueObject a, object b)
        {
            return a.GetType().GetProperties().All(p => PropertyIsEqual(a, b, p));
        }

        private static bool PropertyIsEqual(object a, object b, PropertyInfo propertyInfo)
        {
            return object.Equals(propertyInfo.GetValue(a, null), propertyInfo.GetValue(b, null));
        }

        private static bool FieldIsEqual(object a, object b, FieldInfo fieldInfo)
        {
            return object.Equals(fieldInfo.GetValue(a), fieldInfo.GetValue(b));
        }
    }
}
