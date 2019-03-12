namespace Infrastructure.Scheduler.Tests
{
	using System;
	using System.Linq;

	using FluentAssertions;
	using FluentAssertions.Common;

	using Microsoft.Practices.Unity;

	using NUnit.Framework;

	[TestFixture]
	public sealed class SchedulerJobTypesLoaderTests
	{
		[SetUp]
		public void SetUp()
		{
			_container = new UnityContainer();

			_target = new SchedulerJobTypesLoader(_container);
		}

		private ISchedulerJobTypesLoader _target;

		private IUnityContainer _container;

		private sealed class TestJob : ScheduledJob
		{
			public override TimeSpan Interval => new TimeSpan(0, 0, 0, 1);

			protected override int Process() => 0;
		}

		[Test]
		public void ShouldGetEmptyListWhenNoOneJobRegistered()
		{
			var result = _target.Load();

			result.Should().NotBeNull();
		}

		[Test]
		public void ShouldResolveTwoJobTypesOnDifferentRegistrationNames()
		{
			_container.RegisterType<ScheduledJob, TestJob>("jobname1");
			_container.RegisterType<ScheduledJob, TestJob>("jobname2");

			var result = _target.Load().ToArray();

			result.Length.ShouldBeEquivalentTo(2);
			result[0].IsSameOrEqualTo(result[1]);
		}

		[Test]
		public void ShouldReturnRegisteredByNameJob()
		{
			_container.RegisterType<ScheduledJob, TestJob>(typeof (TestJob).FullName);

			var result = _target.Load().ToArray();

			result.Length.ShouldBeEquivalentTo(1);
		}
	}
}