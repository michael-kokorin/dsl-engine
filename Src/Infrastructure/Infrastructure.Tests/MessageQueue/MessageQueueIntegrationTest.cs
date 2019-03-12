namespace Infrastructure.Tests.MessageQueue
{
	using FluentAssertions;

	using Microsoft.Practices.Unity;

	using NUnit.Framework;

	using Common;
	using Common.Container;
	using Common.Extensions;
	using Common.Transaction;
	using Infrastructure.MessageQueue;
	using Repository;

	[TestFixture]
	[Ignore("Integration tests")]
	public sealed class MessageQueueIntegrationTest
	{
		private IMessageQueue _target;

		private IUnitOfWork _unitOfWork;

		private IUnityContainer _container;

		[SetUp]
		public void SetUp()
		{
			_container = new UnityContainer();

			_container.Register(new CommonContainerModule(), ReuseScope.Container);
			_container.Register(new RepositoryContainerModule(), ReuseScope.Container);
			_container.Register(new InfrastructureContainerModule(), ReuseScope.Container);

			_target = _container.Resolve<IMessageQueue>();

			_unitOfWork = _container.Resolve<IUnitOfWork>();
		}

		[Test]
		public void ShouldWriteMessage()
		{
			using (var writeAction = _target.BeginWrite(MessageQueueKeys.Notifications))
			{
				using (_unitOfWork.BeginTransaction())
				{
					writeAction.Send("hello");
				}

				_unitOfWork.Reset();

				using (var trans = _unitOfWork.BeginTransaction())
				{
					writeAction.Send("hello2");

					_unitOfWork.Commit();

					trans.Commit();
				}
			}
		}

		[Test]
		public void ShouldReceiveMessage()
		{
			using (var writeAction = _target.BeginRead(MessageQueueKeys.Notifications))
			{
				var result = writeAction.Read();

				_unitOfWork.Commit();

				result.Should().NotBeNullOrEmpty();
			}
		}
	}
}