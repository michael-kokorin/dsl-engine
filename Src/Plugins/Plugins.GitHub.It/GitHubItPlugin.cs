namespace Plugins.GitHub.It
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Octokit;

	using Infrastructure.Plugins.Contracts;
	using Plugins.GitHub.It.Extensions;

	using Issue = Infrastructure.Plugins.Contracts.Issue;

	internal sealed class GitHubItPlugin: GitHubPlugin, IIssueTrackerPlugin
	{
		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		public override string Title => "GitHub";

		[NotNull]
		public Issue CreateIssue([NotNull] string branchId, [NotNull] CreateIssueRequest issue)
		{
			var gitHubClient = GetClient();

			var newIssue = new NewIssue(issue.Title) {Body = issue.Description};

			var result = gitHubClient.Issue.Create(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				newIssue)
									.Result;

			return result.ToModel();
		}

		[NotNull]
		public Issue GetIssue([NotNull] string issueId)
		{
			if(issueId == null)
			{
				throw new ArgumentNullException(nameof(issueId));
			}

			var gitHubClient = GetClient();

			var intIssueId = int.Parse(issueId);

			var issue = gitHubClient.Issue.Get(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				intIssueId)
									.Result;

			return issue.ToModel();
		}

		[NotNull]
		[ItemNotNull]
		public IEnumerable<Issue> GetIssues()
		{
			var gitHubClient = GetClient();
			var issues = gitHubClient.Issue.GetAllForRepository(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName)).Result;

			return issues.Select(x => x.ToModel()).ToArray();
		}

		[NotNull]
		public Issue UpdateIssue([NotNull] UpdateIssueRequest updateIssue)
		{
			if(updateIssue == null)
			{
				throw new ArgumentNullException(nameof(updateIssue));
			}

			var gitHubClient = GetClient();

			var intIssueId = int.Parse(updateIssue.Id);

			var issueUpdate = new IssueUpdate
							{
								Body = updateIssue.Description,
								State = updateIssue.Status.ToState(),
								Title = updateIssue.Title
							};

			var result = gitHubClient.Issue.Update(
				GetSetting(GitHubItSettingKeys.RepositoryOwner),
				GetSetting(GitHubItSettingKeys.RepositoryName),
				intIssueId,
				issueUpdate)
									.Result;

			return result.ToModel();
		}
	}
}