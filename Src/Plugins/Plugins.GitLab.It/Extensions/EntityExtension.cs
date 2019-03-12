namespace Plugins.GitLab.It.Extensions
{
	using System;

	using Infrastructure.Plugins.Contracts;

	internal static class EntityExtension
	{
		public static Issue ToDto(this Client.Entity.Issue issue, string host, string repoOwner, string repoName) => new Issue
		{
			AssignedTo = issue.Assignee?.Name,
			Created = issue.CreatedAt,
			CreatedBy = issue.Author.Name,
			Description = issue.Description,
			Id = issue.Iid.ToString(),
			Title = issue.Title,
			Url = $"{host}/{repoOwner}/{repoName}/issues/{issue.Iid}",
			Status = issue.State.GetStatus()
		};

		private static IssueStatus GetStatus(this string source)
		{
			if (string.IsNullOrEmpty(source))
				return IssueStatus.Unknown;

			if (source.Equals("closed")) return IssueStatus.Closed;

			return source.Equals("opened")
				? IssueStatus.Open
				: IssueStatus.Unknown;
		}

		public static string AsString(this IssueStatus status)
		{
			switch (status)
			{
				case IssueStatus.New:
				case IssueStatus.Open:
					return "reopen";
				case IssueStatus.Closed:
					return "close";
				default:
					throw new Exception($"Unsupported GitLab issue status. Status='{status}'");
			}
		}
	}
}