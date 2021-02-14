using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject.Extensions
{

    internal static class ValueObjectImmutabilty
    {
        internal static bool IsImmutable(this ValueObject valueObject, ValueObjectImmutabiltyValidation? validation=null)
        {

            var nonImmutableProperties = FindNonImmutableProperties(valueObject);
            var nonImmutableFields = FindNonImmutableFields(valueObject);

            var isImmutable = PropertiesAreAllImmutable(nonImmutableProperties) && FieldsAreAllImmutable(nonImmutableFields);

            if (validation != null)
            {
                validation.AddValidationErrors(nonImmutableProperties);
                validation.AddValidationErrors(nonImmutableFields);
            }
            return isImmutable;
        }

        internal static bool IsImmutable(this ValueObject valueObject, out ValueObjectImmutabiltyValidation validation)
        {
            validation = new ValueObjectImmutabiltyValidation();

            var isImmutable = IsImmutable(valueObject, validation);

            return isImmutable;
        }

        private static bool PropertiesAreAllImmutable(IReadOnlyList<PropertyInfo> nonImmutableProperties) => !nonImmutableProperties.Any();

        private static bool FieldsAreAllImmutable(IReadOnlyList<FieldInfo> nonImmutableFields) => !nonImmutableFields.Any();

        private static IReadOnlyList<PropertyInfo> FindNonImmutableProperties(ValueObject valueObject)
        {
            var nonImmutableProperties = valueObject.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !IsInitOnly(p));

            return nonImmutableProperties.ToList();
        }

        private static IReadOnlyList<FieldInfo> FindNonImmutableFields(ValueObject valueObject)
        {
            var nonImmutablePublicFields = valueObject.GetType()
                  .GetFields(BindingFlags.Instance | BindingFlags.Public)
                  .Where(p => !p.IsInitOnly);

            return nonImmutablePublicFields.ToList();
        }

        private static bool IsInitOnly(PropertyInfo propertyInfo)
        {
            var setMethod = propertyInfo.SetMethod;

            if (setMethod == null)
                return false;

            var isExternalInitType = typeof(System.Runtime.CompilerServices.IsExternalInit);
            return setMethod.ReturnParameter.GetRequiredCustomModifiers().Contains(isExternalInitType);
        }

    }
}
