namespace Plugins.GitLab.It
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Infrastructure.Plugins.Contracts;
	using Plugins.GitLab.Client;
	using Plugins.GitLab.Client.Api;
	using Plugins.GitLab.Client.Entity;
	using Plugins.GitLab.It.Extensions;

	using Issue = Infrastructure.Plugins.Contracts.Issue;

	// ReSharper disable once UnusedMember.Global
	public sealed class GitLabItPlugin : GitLabPlugin, IIssueTrackerPlugin
	{
		/// <summary>
		///     Creates the issue
		/// </summary>
		/// <param name="branchId">Target branch Id</param>
		/// <param name="createIssueRequest">The issue.</param>
		/// <returns></returns>
		public Issue CreateIssue(string branchId, CreateIssueRequest createIssueRequest)
		{
			if (createIssueRequest == null) throw new ArgumentNullException(nameof(createIssueRequest));

			var client = GetClient();

			var project = GetProject(client);

			var issue = client.Issues
				.Create(project.Id,
					new CreateIssue
					{
						Description = createIssueRequest.Description,
						Title = createIssueRequest.Title
					})
				.Result;

			return ToDto(issue.Data);
		}

		private Issue ToDto(Client.Entity.Issue source) =>
			source.ToDto(GetSetting(GitLabSetting.Host),
				GetSetting(GitLabSetting.RepositoryOwner),
				GetSetting(GitLabSetting.RepositoryName));

		/// <summary>
		///     Gets the issue.
		/// </summary>
		/// <param name="issueId">The issue identifier.</param>
		/// <returns></returns>
		public Issue GetIssue(string issueId)
		{
			var client = GetClient();

			var project = GetProject(client);

			var issue = GetIssue(issueId, client, project);

			return issue == null ? null : ToDto(issue);
		}

		private static Client.Entity.Issue GetIssue(string issueId, GitLabClient client, Project project)
		{
			var issues = client.Issues.Get(project.Id,
				new GetIssue
				{
					Iid = issueId != null ? Convert.ToInt64(issueId) : default(long?)
				}).Result;

			var issue = issues.Data.SingleOrDefault();
			return issue;
		}

		/// <summary>
		///     Gets the issues.
		/// </summary>
		/// <returns>The issues.</returns>
		public IEnumerable<Issue> GetIssues()
		{
			var client = GetClient();

			var project = GetProject(client);

			var issues = client.Issues.Get(project.Id, new GetIssue()).Result;

			return issues.Data.Select(ToDto);
		}

		/// <summary>
		///     Updates the issue.
		/// </summary>
		/// <param name="updateIssueRequest">The update issue.</param>
		/// <returns></returns>
		public Issue UpdateIssue(UpdateIssueRequest updateIssueRequest)
		{
			if (updateIssueRequest == null) throw new ArgumentNullException(nameof(updateIssueRequest));

			var client = GetClient();

			var project = GetProject(client);

			var issue = GetIssue(updateIssueRequest.Id, client, project);

			if (issue == null)
				return null;

			var updatedIssue = client.Issues.Update(
				project.Id,
				issue.Id,
				new UpdateIssue
				{
					Description = updateIssueRequest.Description,
					Title = updateIssueRequest.Title,
					State = updateIssueRequest.Status.AsString()
				}).Result;

			return ToDto(updatedIssue.Data);
		}
	}
}