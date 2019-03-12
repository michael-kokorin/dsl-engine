namespace Plugins.GitHub.It.Extensions
{
	using System;

	using JetBrains.Annotations;

	using Octokit;

	using Infrastructure.Plugins.Contracts;

	using Issue = Infrastructure.Plugins.Contracts.Issue;

	internal static class IssueExtensions
	{
		[NotNull]
		public static Issue ToModel([NotNull] this Octokit.Issue issue) =>
			new Issue
			{
				AssignedTo = issue.Assignee?.Name,
				Created = issue.CreatedAt.DateTime,
				CreatedBy = issue.User?.Name,
				Description = issue.Body,
				Id = issue.Number.ToString(),
				Title = issue.Title,
				Status = issue.State.ToStatus(),
				Url = issue.HtmlUrl?.AbsoluteUri
			};

		public static ItemState ToState(this IssueStatus status)
		{
			switch(status)
			{
				case IssueStatus.Closed:
					return ItemState.Closed;
				case IssueStatus.Open:
					return ItemState.Open;
				case IssueStatus.Unknown:
					return ItemState.All;
				case IssueStatus.New:
					return ItemState.All;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static IssueStatus ToStatus(this ItemState state)
		{
			switch(state)
			{
				case ItemState.Closed:
					return IssueStatus.Closed;
				case ItemState.Open:
					return IssueStatus.Open;
				case ItemState.All:
					return IssueStatus.Unknown;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}