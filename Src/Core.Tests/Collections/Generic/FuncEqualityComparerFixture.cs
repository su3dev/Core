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
		public void Hash_lambda_passed_in_ctor_cannot_be_null()
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

			sut.Equals(instance1, instance2);

			comparerMock.Verify(m => m.Invoke(instance1, instance2), Times.Once);
	    }

	    [Fact]
        public void GetHashCode_uses_default_implementation_when_not_specified()
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

			sut.Equals(instance1, instance2);

			comparerMock.Verify(m => m.Invoke(instance1, instance2), Times.Once);
	    }

	    [Fact]
	    public void GetHashCode_uses_specified_hash_lambda()
	    {
			var hashMock = new Mock<Func<SomeType, int>>();

			var sut = new FuncEqualityComparer<SomeType>((_, _) => true, hashMock.Object);

			var instance = new SomeType();
			sut.GetHashCode(instance);

			hashMock.Verify(m => m.Invoke(instance), Times.Once);
	    }
    }
}
