using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using su3dev.Collections.Generic;
using Xunit;

namespace Core.Tests.Collections.Generic
{
    public class FuncEqualityComparerFixture
	{
        [Fact]
        public void Implements_EqualityComparer()
        {
            var sut = new FuncEqualityComparer<SomeType>((_, _) => true);
			sut.Should().BeAssignableTo<EqualityComparer<SomeType>>();
        }

		[Fact]
		public void Comparer_passed_in_ctor_cannot_be_null()
		{
			Assert.Throws<ArgumentNullException>(() => new FuncEqualityComparer<SomeType>(null));
		}

		[Fact]
		public void Hasher_passed_in_ctor_cannot_be_null()
		{
			Assert.Throws<ArgumentNullException>(() => new FuncEqualityComparer<SomeType>((_, _) => true, null));
		}

		[Fact]
	    public void Equals_invokes_comparer_specified_in_single_parameter_ctor()
	    {
			var comparerMock = new Mock<Func<SomeType, SomeType, bool>>();

			var sut = new FuncEqualityComparer<SomeType>(comparerMock.Object);

			var instance1 = new SomeType();
			var instance2 = new SomeType();

			_ = sut.Equals(instance1, instance2);

			comparerMock.Verify(m => m.Invoke(instance1, instance2), Times.Once);
	    }

	    [Fact]
        public void GetHashCode_uses_default_hasher_when_not_specified()
	    {
			var sut = new FuncEqualityComparer<Guid>((_, _) => true);

			var someGuid = Guid.NewGuid();
			var actual = sut.GetHashCode(someGuid);

			actual.Should().Be(someGuid.GetHashCode());
	    }

	    [Fact]
	    public void Equals_invokes_comparer_specified_in_two_parameter_ctor()
	    {
			var comparerMock = new Mock<Func<SomeType, SomeType, bool>>();

			var sut = new FuncEqualityComparer<SomeType>(comparerMock.Object, _ => 0);

			var instance1 = new SomeType();
			var instance2 = new SomeType();

			_ = sut.Equals(instance1, instance2);

			comparerMock.Verify(m => m.Invoke(instance1, instance2), Times.Once);
	    }

	    [Fact]
	    public void GetHashCode_uses_specified_hasher()
	    {
			var hashMock = new Mock<Func<SomeType, int>>();

			var sut = new FuncEqualityComparer<SomeType>((_, _) => true, hashMock.Object);

			var instance = new SomeType();
			_ = sut.GetHashCode(instance);

			hashMock.Verify(m => m.Invoke(instance), Times.Once);
	    }

        [Theory]
        [InlineData(null, "abc", false)]
        [InlineData("abc", null, false)]
        [InlineData(null, null, true)]
        public void Equals_can_take_nulls(string instance1, string instance2, bool expected)
        {
            var sut = new FuncEqualityComparer<string>(EqualityComparer<string>.Default.Equals);

            var actual = sut.Equals(instance1, instance2);

            actual.Should().Be(expected);
        }

        [Fact]
        public void GetHashCode_on_null_with_default_hasher_throws()
        {
            var sut = new FuncEqualityComparer<string>(EqualityComparer<string>.Default.Equals);

            // ReSharper disable HeapView.BoxingAllocation
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => sut.GetHashCode(null));
            // ReSharper restore AssignNullToNotNullAttribute
            // ReSharper restore HeapView.BoxingAllocation
        }
    }
}
