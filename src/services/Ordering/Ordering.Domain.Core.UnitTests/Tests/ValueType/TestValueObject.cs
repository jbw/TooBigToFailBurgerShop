using System;
using Xunit;
using Shouldly;

namespace Ordering.Domain.Core.UnitTests
{
    public class TestValueObject
    {

        [Fact]
        public void Should_Throw_When_ValueObject_Is_Not_Immutable()
        {
            Assert.ThrowsAny<Exception>(() =>
            {
                new NotImmutablePureValueObject();
            });
        }

        [Fact]
        public void Should_Be_Same_Type()
        {
            var address = new Address("", "");
            var sameAddress = new Address("", "");

            address.ShouldBeOfType<Address>();
            sameAddress.ShouldBeOfType<Address>();
        }

        [Fact]
        public void Should_Be_Equal()
        {
            var address = new Address("A", "A");
            var sameAddress = new Address("A", "A");

            address.ShouldBe(sameAddress);
        }


        [Fact]
        public void Should_Not_Be_Equal()
        {
            var address = new Address("A", "B");
            var differentAddress = new Address("B", "B");

            address.ShouldNotBe(differentAddress);
        }

        [Fact]
        public void Same_Values_Different_Type_Should_Not_Be_Equal()
        {
            var address = new Address("A", "A");
            var differentAddress = new CopyOfAddress("A", "A");

            address.Equals(differentAddress).ShouldBe(false);
        }

        [Fact]
        public void Null_Should_Not_Be_Equal()
        {
            var address = new Address("A", "A");

            address.Equals(null).ShouldBe(false);
        }

        [Fact]
        public void Subclassed_ValueObject_Should_Not_Be_Equal()
        {
            var address = new Address("A", "A");
            var subClassOfAddress = new SubClassOfAddress("A", "A");

            address.Equals(subClassOfAddress).ShouldBe(false);
        }
    }
}
