namespace Plugins.Tfs.It.Tests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore]
	public sealed class TfsItPluginTest
	{
		[SetUp]
		public void SetUp()
		{
			_target = new TfsItPlugin();

			_target.LoadSettingValues(
				new Dictionary<string, string>
				{
					{
						TfsSettings.UserName.ToString(),
						"msharonov"
					},
					{
						TfsSettings.UserPassword.ToString(),
						"P@ssw0rd"
					},
					{
						TfsSettings.HostName.ToString(),
						"https://veises.visualstudio.com/DefaultCollection/"
					},
					{
						TfsSettings.Project.ToString(),
						"$vcs$"
					}
				});
		}

		private IIssueTrackerPlugin _target;

		[Test]
		public void ShouldCreateIssueInTfs()
		{
			var message = DateTime.Now.ToString("F");

			var result = _target.CreateIssue(
				"$/Mopas/Src/master",
				new CreateIssueRequest
				{
					Description = message,
					Title = message
				});

			result.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetIssueFromTfs()
		{
			var issue = _target.GetIssue("3");

			issue.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetIssueListFromTfs()
		{
			var issue = _target.GetIssues();

			issue.Should().NotBeNull();
		}

		[Test]
		public void ShouldUpdateIssueInTfs()
		{
			var message = DateTime.Now.ToString("F");

			var result = _target.UpdateIssue(
				new UpdateIssueRequest
				{
					Description = message,
					Id = 3.ToString(),
					Status = IssueStatus.Open,
					Title = message
				});

			result.Should().NotBeNull();
		}
	}
}