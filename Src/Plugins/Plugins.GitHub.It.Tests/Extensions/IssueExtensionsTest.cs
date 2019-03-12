namespace Plugins.GitHub.It.Tests.Extensions
{
    using FluentAssertions;

    using NUnit.Framework;

    using Octokit;

    using Infrastructure.Plugins.Contracts;
    using Plugins.GitHub.It.Extensions;

    [TestFixture]
    public sealed class IssueExtensionsTest
    {
        [TestCase(ItemState.Closed, ExpectedResult = IssueStatus.Closed)]
        [TestCase(ItemState.All, ExpectedResult = IssueStatus.Unknown)]
        [TestCase(ItemState.Open, ExpectedResult = IssueStatus.Open)]
        public IssueStatus ShouldConvertItemState(ItemState state) => state.ToStatus();

        [TestCase(IssueStatus.Closed, ExpectedResult = ItemState.Closed)]
        [TestCase(IssueStatus.Open, ExpectedResult = ItemState.Open)]
        [TestCase(IssueStatus.Unknown, ExpectedResult = ItemState.All)]
        [TestCase(IssueStatus.New, ExpectedResult = ItemState.All)]
        public ItemState ShouldConvertIssueStatus(IssueStatus status) => status.ToState();

        [Test]
        public void ShouldConvertEmptyIssueToModel()
        {
            var issue = new Octokit.Issue();

            var result = issue.ToModel();

            result.Should().NotBeNull();
        }
    }
}