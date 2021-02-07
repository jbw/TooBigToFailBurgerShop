using System;
using System.Linq;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject.Extensions;

namespace TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject
{
    public abstract class PureValueObject : ValueObject
    {
        protected PureValueObject() : base()
        {
            ValueObjectImmutabiltyValidation validation;

            if (!this.IsImmutable(out validation)) RaiseNotImmutableException(validation);
       
        }

        private void RaiseNotImmutableException(ValueObjectImmutabiltyValidation validation)
        {

            var aggregateException = new AggregateException(
                $"{GetType().Name} not immutable",
                validation.ValidationErrors.Select(e => new Exception(e))
            );

            throw aggregateException;
        }
    }
}
