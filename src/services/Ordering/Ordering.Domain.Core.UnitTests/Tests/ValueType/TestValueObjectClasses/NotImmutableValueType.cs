using System;
using TooBigToFailBurgerShop.Ordering.Domain.Core.SeedWork.ValueObject;

namespace Ordering.Domain.Core.UnitTests
{
    public class NotImmutablePureValueObject : PureValueObject
    {
        public DateTime DateA { get { return DateTime.Now; } }
        public DateTime DateB { get; set; }
        public DateTime DateC { get; set; } = DateTime.Now;
        public DateTime Date = DateTime.Now; 
        public string Name = "Jason";
    }

}
