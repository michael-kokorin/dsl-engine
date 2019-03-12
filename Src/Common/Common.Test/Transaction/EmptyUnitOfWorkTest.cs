namespace Common.Tests.Transaction
{
	using FluentAssertions;

	using NUnit.Framework;

	using Common.Transaction;

	[TestFixture]
	public sealed class EmptyUnitOfWorkTest
	{
		[Test]
		public void BeginTransactionReturnsEmptyTransaction()
		{
			var entity = new EmptyUnitOfWork();

			var result = entity.BeginTransaction();

			result.Should().NotBeNull();
			result.Should().BeOfType<EmptyTransaction>();
		}

		[Test]
		public void CommitDoesNotThrown()
		{
			var entity = new EmptyUnitOfWork();

			Assert.DoesNotThrow(() => entity.Commit());
		}

		[Test]
		public void DisposeDoesNotThrown()
		{
			var entity = new EmptyUnitOfWork();

			Assert.DoesNotThrow(() => entity.Dispose());
		}

		[Test]
		public void ResetDoesNotThrown()
		{
			var entity = new EmptyUnitOfWork();

			Assert.DoesNotThrow(() => entity.Reset());
		}
	}
}