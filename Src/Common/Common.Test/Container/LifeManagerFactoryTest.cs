namespace Common.Tests.Container
{
	using System;

	using Microsoft.Practices.Unity;

	using NUnit.Framework;

	using Common.Container;

	[TestFixture]
	public sealed class LifeManagerFactoryTest
	{
		[SetUp]
		public void SetUp() => _target = new LifeManagerFactory();

		private ILifeManagerFactory _target;

		[TestCase(ReuseScope.Container, ExpectedResult = typeof(ContainerControlledLifetimeManager))]
		[TestCase(ReuseScope.External, ExpectedResult = typeof(ExternallyControlledLifetimeManager))]
		[TestCase(ReuseScope.Hierarchy, ExpectedResult = typeof(HierarchicalLifetimeManager))]
		[TestCase(ReuseScope.PerResolve, ExpectedResult = typeof(PerResolveLifetimeManager))]
		[TestCase(ReuseScope.PerRequest, ExpectedResult = typeof(PerRequestLifetimeManager))]
		[TestCase(ReuseScope.PerThread, ExpectedResult = typeof(PerThreadLifetimeManager))]
		public Type ShouldBuildLifeTimeManagerForReuseScope(ReuseScope reuseScope) => _target.Build(reuseScope).GetType();
	}
}