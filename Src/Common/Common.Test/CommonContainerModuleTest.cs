namespace Common.Tests
{
	using FluentAssertions;

	using Microsoft.Practices.Unity;

	using NUnit.Framework;

	using Common.Container;
	using Common.Logging;
	using Common.Time;

	[TestFixture]
	public sealed class CommonContainerModuleTest
	{
		[SetUp]
		public void SetUp()
		{
			_target = new UnityContainer();

			new CommonContainerModule().Register(_target, ReuseScope.Container);

			_child = _target.CreateChildContainer();
		}

		private IUnityContainer _target;

		private IUnityContainer _child;

		[Test]
		public void ShouldResolveLogAsHierarchy()
		{
			var parentLogService = _target.Resolve<ILog>();

			var childLogService = _child.Resolve<ILog>();

			childLogService.Should().BeSameAs(parentLogService);
		}

		[Test]
		public void ShouldResolveTimeServiceAsContainer()
		{
			var parentTimeService = _target.Resolve<ITimeService>();

			var childTimeService = _child.Resolve<ITimeService>();

			childTimeService.Should().BeSameAs(parentTimeService);
		}
	}
}