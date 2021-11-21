using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using su3dev.Collections.Generic;
using Xunit;

namespace Core.Tests.Collections.Generic
{
	public class EqualityComparerFixture
	{
		[Fact]
		public void Comparer_cannot_be_null()
		{
			Assert.Throws<ArgumentNullException>(() => EqualityComparer.Create((Func<SomeType, SomeType, bool>)null));
		}

		[Fact]
	    public void Equals_uses_specified_comparer()
	    {
			var comparerMock = new Mock<Func<SomeType, SomeType, bool>>();

			var sut = EqualityComparer.Create(comparerMock.Object);

			var instance1 = new SomeType();
			var instance2 = new SomeType();

			sut.Equals(instance1, instance2);

			comparerMock.Verify(m => m.Invoke(instance1, instance2), Times.Once);
		}

		[Fact]
		public void GetHashCode_uses_specified_hash_lambda()
		{
			var hashMock = new Mock<Func<SomeType, int>>();

			var sut = EqualityComparer.Create((_, _) => true, hashMock.Object);

			var instance = new SomeType();
			sut.GetHashCode(instance);

			hashMock.Verify(m => m.Invoke(instance), Times.Once);
		}

	    [Fact]
	    public void GetHashCode_uses_default_implementation_when_not_specified()
	    {
	        var sut = EqualityComparer.Create<Guid>((_, _) => true);

			var someGuid = Guid.NewGuid();
	        var actual = sut.GetHashCode(someGuid);

			actual.Should().Be(someGuid.GetHashCode());
	    }

		[Fact]
		public void Create_returns_IEqualityComparer()
		{
			var sut = EqualityComparer.Create<SomeType>((_, _) => true);

			sut.Should().BeAssignableTo<IEqualityComparer<SomeType>>();
		}
	}
}
