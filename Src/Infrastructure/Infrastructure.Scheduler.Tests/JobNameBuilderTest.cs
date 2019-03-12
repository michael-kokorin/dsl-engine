namespace Infrastructure.Scheduler.Tests
{
    using FluentAssertions;

    using NUnit.Framework;

	[TestFixture]
    public sealed class JobNameBuilderTest
    {
        [SetUp]
        public void SetUp() => _target = new JobNameBuilder();

        private IJobNameBuilder _target;

        private sealed class TestJob : ScheduledJob
        {
            protected override int Process() => 0;
        }

        [Test]
        public void ShouldReturnJobNameWithPrefix()
        {
            var job = new TestJob();

            var result = _target.GetJobName(job);

            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo("Job.TestJob");
        }
    }
}