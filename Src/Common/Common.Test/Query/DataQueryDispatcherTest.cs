namespace Common.Tests.Query
{
	using JetBrains.Annotations;

	using Moq;

	using NUnit.Framework;

	using Common.Query;

	[TestFixture]
	public sealed class DataQueryDispatcherTest
	{
		[SetUp]
		public void SetUp()
		{
			_dataQueryHandlerProvider = new Mock<IDataQueryHandlerProvider>();

			_target = new DataQueryDispatcher(_dataQueryHandlerProvider.Object);
		}

		private IDataQueryDispatcher _target;

		private Mock<IDataQueryHandlerProvider> _dataQueryHandlerProvider;

		[UsedImplicitly]
		public sealed class TestQuery: IDataQuery
		{
		}

		[Test]
		public void ShouldExecureQueryHandler()
		{
			var handler = new Mock<IDataQueryHandler<TestQuery, long>>();

			_dataQueryHandlerProvider.Setup(_ => _.Resolve<TestQuery, long>()).Returns(handler.Object);

			var testQuery = new TestQuery();

			_target.Process<TestQuery, long>(testQuery);

			_dataQueryHandlerProvider.Verify(_ => _.Resolve<TestQuery, long>(), Times.Once);

			handler.Verify(_ => _.Execute(testQuery), Times.Once);
		}
	}
}