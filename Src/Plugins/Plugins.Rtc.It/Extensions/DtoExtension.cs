namespace Plugins.Rtc.It.Extensions
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	using Infrastructure.Plugins.Contracts;
	using Plugins.Rtc.It.Client.Entity;

	using Issue = Infrastructure.Plugins.Contracts.Issue;

	internal static class DtoExtension
	{
		public static Issue ToDto(this Client.Entity.Issue issue, IEnumerable<IssueState> states) =>
			new Issue
			{
				AssignedTo = null,
				Created = issue.Created,
				CreatedBy = null,
				Description = issue.Description,
				Id = issue.Id.ToString(),
				Status = issue.State.Resource.ToDto(states),
				Title = issue.Title,
				Url = null
			};

		private static IssueStatus ToDto(this string state, IEnumerable<IssueState> states)
		{
			var mappedState = states?.SingleOrDefault(_ => state.Equals(_.Resource));

			if (mappedState == null)
				return IssueStatus.Unknown;

			switch (mappedState.Group)
			{
				case WorkItemStates.Open:
					return IssueStatus.New;
				case WorkItemStates.Closed:
					return IssueStatus.Closed;
				case WorkItemStates.InProgress:
					return IssueStatus.Open;
				default:
					return IssueStatus.Unknown;
			}
		}

		public static string ToDto(this IssueStatus status, IEnumerable<IssueState> states)
		{
			string equalState = null;

			switch (status)
			{
				case IssueStatus.New:
					equalState = WorkItemStates.Open;
					break;
				case IssueStatus.Closed:
					equalState = WorkItemStates.Closed;
					break;
				case IssueStatus.Open:
					equalState = WorkItemStates.InProgress;
					break;
			}

			if (equalState == null)
				throw new InvalidEnumArgumentException(nameof(states));

			return states.First(_ => _.Group.Equals(equalState)).Resource;
		}
	}
}