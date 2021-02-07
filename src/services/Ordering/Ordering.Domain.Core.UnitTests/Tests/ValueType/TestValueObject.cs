using FluentAssertions;
using System;
using Xunit;

namespace Ordering.Domain.Core.UnitTests
{
    public class TestValueObject
    {

        [Fact]
        public void Should_Throw_When_ValueObject_Is_Not_Immutable()
        {
            Action initValueObject = () => new NotImmutablePureValueObject();

            initValueObject
                .Should()
                .Throw<AggregateException>()
                .And.InnerExceptions.Count.Should().Be(5);
        }

        [Fact]
        public void Should_Be_Equal()
        {
            var address = new Address("A", "A");
            var sameAddress = new Address("A", "A");

            Assert.True(address == sameAddress);
        }


        [Fact]
        public void Should_Not_Be_Equal()
        {
            var address = new Address("A", "B");
            var differentAddress = new Address("B", "B");

            Assert.True(address != differentAddress);
        }

        [Fact]
        public void Same_Values_Different_Type_Should_Not_Be_Equal()
        {
            var address = new Address("A", "A");
            var differentAddress = new CopyOfAddress("A", "A");

            Assert.True(address != differentAddress);
        }

        [Fact]
        public void Null_Should_Not_Be_Equal()
        {
            var address = new Address("A", "A");

            Assert.True(address != null);
        }

        [Fact]
        public void Subclassed_ValueObject_Should_Not_Be_Equal()
        {
            var address = new Address("A", "A");
            var subClassOfAddress = new SubClassOfAddress("A", "A");

            Assert.True(address != subClassOfAddress);
        }
    }
}
