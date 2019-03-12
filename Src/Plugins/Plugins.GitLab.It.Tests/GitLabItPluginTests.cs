namespace Plugins.GitLab.It.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("Integration tests")]
	public sealed class GitLabItPluginTests
	{
		private IIssueTrackerPlugin _target;

		[SetUp]
		public void SetUp()
		{
			_target = new GitLabItPlugin();

			_target.LoadSettingValues(new Dictionary<string, string>
			{
				{
					GitLabSetting.Token.ToString(),
					"z_F1h_F3JRxPThbhYBoZ"
				},
				{
					GitLabSetting.RepositoryName.ToString(),
					"Test"
				},
				{
					GitLabSetting.RepositoryOwner.ToString(),
					"msharonov"
				},
				{
					GitLabSetting.Host.ToString(),
					"https://gitlab.com"
				}
			});
		}

		[Test]
		public void ShouldGetIssueFromGitLab()
		{
			var issues = _target.GetIssues();

			issues.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void SouldGetSingleIssueFromGitLab()
		{
			var issue = _target.GetIssue("1");

			issue.Should().NotBeNull();
		}

		[Test]
		public void SouldNotGetSingleIssueFromGitLab()
		{
			var issue = _target.GetIssue("4");

			issue.Should().BeNull();
		}

		[Test]
		public void ShouldCreateGitLabIssue()
		{
			var issue = _target.CreateIssue("master", new CreateIssueRequest
			{
				Description = "descr",
				Title = "Some title"
			});

			issue.Should().NotBeNull();
		}

		[Test]
		public void ShouldUpdateGitLabIssue()
		{
			var currentDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

			var issue = _target.UpdateIssue(new UpdateIssueRequest
			{
				Description = currentDate,
				Id = "2",
				Status = IssueStatus.Closed,
				Title = currentDate
			});

			issue.Should().NotBeNull();
		}
	}
}