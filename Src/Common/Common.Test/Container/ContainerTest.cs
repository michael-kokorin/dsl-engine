namespace Common.Tests.Container
{
	using FluentAssertions;

	using JetBrains.Annotations;

	using NUnit.Framework;

	using Common.Container;

	[TestFixture]
	public sealed class ContainerTest
	{
		private IContainer _target;

		[SetUp]
		public void SetUp() => _target = new Container();

		private interface IInterface
		{

		}

		[UsedImplicitly]
		private sealed class MyClass : IInterface
		{

		}

		[Test]
		public void ShouldResolveItself()
		{
			var result = _target.Resolve<IContainer>();

			result.ShouldBeEquivalentTo(_target);
		}

		[Test]
		public void ShouldResolveChildContainer()
		{
			var parentContainerRef = _target.Resolve<IContainer>();

			var childContainer = _target.CreateChild();

			var childContainerRef = childContainer.Resolve<IContainer>();

			parentContainerRef.Should().NotBeSameAs(childContainerRef);
		}

		[Test]
		public void ShouldResolveEquialRefs()
		{
			_target.RegisterType<IInterface, MyClass>(ReuseScope.Container);

			var inst1 = _target.Resolve<IInterface>();
			var inst2 = _target.Resolve<IInterface>();

			inst1.ShouldBeEquivalentTo(inst2);
		}

		[Test]
		public void ShouldResolveEquiaChildlRefs()
		{
			_target.RegisterType<IInterface, MyClass>(ReuseScope.Container);

			var inst1 = _target.Resolve<IInterface>();

			var secondContainer = _target.CreateChild();

			var inst2 = secondContainer.Resolve<IInterface>();

			inst1.ShouldBeEquivalentTo(inst2);
		}
	}
}