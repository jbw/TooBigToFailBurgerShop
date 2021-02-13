using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject.Extensions
{
    internal class ValueObjectImmutabiltyValidation
    {
        public List<string> ValidationErrors { get; init; }

        public ValueObjectImmutabiltyValidation()
        {
            ValidationErrors = new List<string>();
        }

        internal void AddValidationErrors(IReadOnlyList<PropertyInfo> nonImmutableProperties)
        {
            ValidationErrors.AddRange(nonImmutableProperties.Select(p => $"Property: '{p.Name}' is not immutable."));
        }

        internal void AddValidationErrors(IReadOnlyList<FieldInfo> nonImmutableFields)
        {
            ValidationErrors.AddRange(nonImmutableFields.Select(p => $"Property: '{p.Name}' is not immutable."));
        }


    }
}
