namespace Plugins.GitHub.It.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Plugins.Contracts;

	[TestFixture]
	[Ignore("Functional tests. Running on live GitHub enviroment.")]
	public sealed class GitHubItPluginTest
	{
		[SetUp]
		public void SetUp()
		{
			_target = new GitHubItPlugin();

			_target.LoadSettingValues(GetParameters());
		}

		private GitHubItPlugin _target;

		private static Dictionary<string, string> GetParameters() => new Dictionary<string, string>
		{
			{
				GitHubItSettingKeys.ClientToken.ToString(),
				"59df38832087383830e406addec1691c9056eac8"
			},
			{
				GitHubItSettingKeys.RepositoryOwner.ToString(),
				"msharonov"
			},
			{
				GitHubItSettingKeys.RepositoryName.ToString(),
				"sdl_repo"
			}
		};

		[Test]
		public void ShouldCreateIssue()
		{
			var dateTimeString = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

			var title = $"Test issue {dateTimeString}";

			var createIssueRequest = new CreateIssueRequest
			{
				Description = dateTimeString,
				Title = title
			};

			var result = _target.CreateIssue(null, createIssueRequest);

			result.Should().NotBeNull();
			result.Description.ShouldBeEquivalentTo(dateTimeString);
			result.Title.ShouldBeEquivalentTo(title);
		}

		[Test]
		public void ShouldGetCurrentUser()
		{
			var currentUser = _target.GetCurrentUser();

			currentUser.Should().NotBeNull();
		}

		[Test]
		public void ShouldGetissueFromGitHub()
		{
			var result = _target.GetIssue(1.ToString());

			result.Should().NotBeNull();
		}

		[Test]
		public void ShouldReturnSettings()
		{
			var result = _target.GetSettings();

			result.Should().NotBeNull();
			result.SettingDefinitions.Should().HaveCount(4);
		}

		[Test]
		public void ShouldReturnTitle()
		{
			var result = _target.Title;

			result.Should().NotBeNullOrEmpty();
			result.Should().BeEquivalentTo("GitHub Issue Traker");
		}

		[Test]
		public void ShouldUpdateIssueOnGitHub()
		{
			var dateTimeString = DateTime.Now.ToString(CultureInfo.InvariantCulture);

			var updateRequest = new UpdateIssueRequest
			{
				Description = $"Updated in {dateTimeString}",
				Id = 1.ToString(),
				Status = IssueStatus.Closed,
				Title = dateTimeString
			};

			var result = _target.UpdateIssue(updateRequest);

			result.Should().NotBeNull();
			result.Description.ShouldBeEquivalentTo(updateRequest.Description);
			result.Status.ShouldBeEquivalentTo(updateRequest.Status);
			result.Title.ShouldBeEquivalentTo(updateRequest.Title);
		}
	}
}