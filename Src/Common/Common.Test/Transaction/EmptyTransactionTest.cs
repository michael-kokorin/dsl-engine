namespace Common.Tests.Transaction
{
	using NUnit.Framework;

	using Common.Transaction;

	[TestFixture]
	public sealed class EmptyTransactionTest
	{
		[Test]
		public void CommitDoesNotThrowException()
		{
			var entity = new EmptyTransaction();

			Assert.DoesNotThrow(() => entity.Commit());
		}

		[Test]
		public void DisposeDoesNotThrowException()
		{
			var entity = new EmptyTransaction();

			Assert.DoesNotThrow(() => entity.Dispose());
		}
	}
}