namespace Plugins.Tfs.It
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Microsoft.TeamFoundation.WorkItemTracking.Client;

	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.Plugins.Contracts;

	public sealed class TfsItPlugin: TfsPlugin, IIssueTrackerPlugin
	{
		private const string UseVcsBranch = "$vcs$";

		public override string Title => "Team Foundation Server";

		public override PluginSettingGroupDefinition GetSettings()
		{
			var settings = base.GetSettings();

			settings.SettingDefinitions.Add(
				new Infrastructure.Plugins.Common.Contracts.PluginSettingDefinition
				{
					DisplayName = "Project",
					Code = TfsSettings.Project.ToString(),
					SettingType = SettingType.Text,
					SettingOwner = SettingOwner.Project,
					IsAuthentication = true
				});

			return settings;
		}

		/// <summary>
		///     Creates the issue
		/// </summary>
		/// <param name="branchId">Target branch Id</param>
		/// <param name="issue">The issue.</param>
		/// <returns>Created issue.</returns>
		[NotNull]
		public Issue CreateIssue([NotNull] string branchId, [NotNull] CreateIssueRequest issue)
		{
			if(issue == null)
			{
				throw new ArgumentNullException(nameof(issue));
			}

			var projectName = GetSetting(TfsSettings.Project);

			var workItemStore = GetStore();

			Project project;

			if(projectName.Equals(UseVcsBranch, StringComparison.InvariantCultureIgnoreCase))
			{
				if(string.IsNullOrEmpty(branchId))
				{
					throw new ArgumentException(nameof(branchId));
				}

				var projects = workItemStore.Projects;

				project = projects
					.Cast<Project>()
					.SingleOrDefault(_ => branchId.StartsWith($"$/{_.Name}", StringComparison.Ordinal));
			}
			else
			{
				project = workItemStore.Projects[projectName];
			}

			if(project == null)
			{
				throw new Exception();
			}

			const string taskItemTypeName = "Task";

			var workItemType = project.WorkItemTypes[taskItemTypeName];

			var workItem = new WorkItem(workItemType)
							{
								Description = issue.Description,
								Title = issue.Title
							};

			var validationResult = workItem.Validate();

			if(validationResult.Count > 0)
			{
				throw new Exception(); // TODO: specific exception
			}

			workItem.Save();

			return GetIssue(workItem);
		}

		/// <summary>
		///     Gets the issue.
		/// </summary>
		/// <param name="issueId">The issue identifier.</param>
		/// <returns>Issue.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="issueId"/> is <see langword="null"/> or empty.</exception>
		/// <exception cref="System.Exception">Work item is not found.</exception>
		[NotNull]
		public Issue GetIssue([NotNull] string issueId)
		{
			if(string.IsNullOrEmpty(issueId))
			{
				throw new ArgumentNullException(nameof(issueId));
			}

			var tpc = GetClient();

			var workItemStore = tpc.GetService<WorkItemStore>();

			var issueIdNumber = int.Parse(issueId);

			var workItem = workItemStore.GetWorkItem(issueIdNumber);

			if(workItem == null)
			{
				throw new Exception($"Work item with {issueIdNumber} id is not found");
			}

			return GetIssue(workItem);
		}

		[NotNull]
		[ItemNotNull]
		public IEnumerable<Issue> GetIssues()
		{
			var workItemStore = GetStore();

			// very simple query for all work items
			var query = new Query(workItemStore, "select * from issue");

			var workItemCollection = query.RunQuery();

			return workItemCollection
				.Cast<WorkItem>()
				.Select(GetIssue);
		}

		[NotNull]
		public Issue UpdateIssue([NotNull] UpdateIssueRequest updateIssue)
		{
			if(updateIssue == null)
			{
				throw new ArgumentNullException(nameof(updateIssue));
			}

			var wis = GetStore();

			var workItemidNumber = int.Parse(updateIssue.Id);

			var workItem = wis.GetWorkItem(workItemidNumber);

			if (workItem == null)
				throw new TeamFoundationIssueDoesNotExistsException(GetSetting(TfsSettings.HostName),
					GetSetting(TfsSettings.Project),
					updateIssue.Id);

			workItem.Title = updateIssue.Title;
			workItem.Description = updateIssue.Description;

			switch(updateIssue.Status)
			{
				case IssueStatus.Closed:
					if(workItem.State != WorkItemStatuses.Closed)
					{
						workItem.State = WorkItemStatuses.Closed;
					}
					break;
				case IssueStatus.Open:
					if(workItem.State == WorkItemStatuses.Closed)
					{
						workItem.State = WorkItemStatuses.Active;
					}
					break;
				case IssueStatus.Unknown:
					break;
				case IssueStatus.New:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			workItem.Save();

			return GetIssue(workItem);
		}

		[NotNull]
		private static Issue GetIssue([NotNull] WorkItem workItem)
		{
			var assignedToField = workItem.Fields[WorkItemFields.AssignedTo];

			var issue = new Issue
			{
				AssignedTo = assignedToField?.Value.ToString(),
				CreatedBy = workItem.CreatedBy,
				Created = workItem.CreatedDate,
				Description = workItem.Description,
				Id = workItem.Id.ToString(),
				Status = GetStatus(workItem),
				Title = workItem.Title,
				Url = workItem.Uri.ToString()
			};

			return issue;
		}

		private static IssueStatus GetStatus([NotNull] WorkItem workItem)
		{
			switch(workItem.State)
			{
				case WorkItemStatuses.New:
				case WorkItemStatuses.Proposed:
					return IssueStatus.New;
				case WorkItemStatuses.Accepted:
				case WorkItemStatuses.Active:
				case WorkItemStatuses.Design:
				case WorkItemStatuses.InPlanning:
				case WorkItemStatuses.InProgress:
				case WorkItemStatuses.Inactive:
				case WorkItemStatuses.Ready:
				case WorkItemStatuses.Removed:
				case WorkItemStatuses.Requested:
				case WorkItemStatuses.Resolved:
				case WorkItemStatuses.Completed:
					return IssueStatus.Open;
				case WorkItemStatuses.Closed:
					return IssueStatus.Closed;
				default:
					return IssueStatus.Unknown;
			}
		}

		private WorkItemStore GetStore()
		{
			var tpc = GetClient();

			return tpc.GetService<WorkItemStore>();
		}
	}
}