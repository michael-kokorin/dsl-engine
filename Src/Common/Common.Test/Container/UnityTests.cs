namespace Common.Tests.Container
{
	using FluentAssertions;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using NUnit.Framework;

	using Common.Container;
	using Common.Extensions;

	[TestFixture]
	public sealed class UnityTest
	{
		[SetUp]
		public void SetUp() => _target = new UnityContainer();

		private IUnityContainer _target;

		private interface ITestInterface
		{
		}

		[UsedImplicitly]
		private sealed class Test: ITestInterface
		{
		}

		[Test]
		public void ShouldResolveDifferentInstances()
		{
			_target.RegisterType<ITestInterface, Test>(ReuseScope.Hierarchy);

			var childContainer = _target.CreateChildContainer();

			var inst1 = _target.Resolve<ITestInterface>();
			var inst2 = childContainer.Resolve<ITestInterface>();

			inst1.Should().NotBeSameAs(inst2);
		}

		[Test]
		public void ShouldResolveSameInstances()
		{
			_target.RegisterType<ITestInterface, Test>(ReuseScope.Container);

			var childContainer = _target.CreateChildContainer();

			var inst1 = _target.Resolve<ITestInterface>();
			var inst2 = childContainer.Resolve<ITestInterface>();

			inst1.Should().BeSameAs(inst2);
		}
	}
}