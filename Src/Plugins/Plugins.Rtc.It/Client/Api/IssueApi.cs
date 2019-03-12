namespace Plugins.Rtc.It.Client.Api
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Plugins.Rtc.It.Client.Entity;

	public sealed class IssueApi : BaseApi
	{
		internal IssueApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<Issue>> Create(string projectUuid, CreateWorkItem createWorkItem)
		{
			var request = RequestFactory.Create("oslc/contexts/{projectUuid}/workitems.json", HttpMethod.Post);

			request.AddUrlSegment("projectUuid", projectUuid);
			request.SetDataFormat(DataFormat.Json);

			var createIssue = new ChangeIssue
			{
				Description = createWorkItem.Description,
				FiledAgainst = new ResourceLink
				{
					Resource = createWorkItem.FiledAgainstResource
				},
				Title = createWorkItem.Title,
				Type = new ResourceLink
				{
					Resource = createWorkItem.TypeResource
				}
			};

			request.AddBody(createIssue);

			return await request.Execute<Issue>();
		}

		public async Task<RequestResult<Issue>> Get(long issueId)
		{
			var request = RequestFactory.Create("oslc/workitems/{issueId}.json");

			request.AddUrlSegment("issueId", issueId);

			return await request.Execute<Issue>();
		}

		public async Task<RequestResult<EntityList<Issue>>> GetByProject(string projectUuid)
		{
			var request = RequestFactory.Create("oslc/contexts/{projectUuid}/workitems.json");

			request.AddUrlSegment("projectUuid", projectUuid);

			return await request.Execute<EntityList<Issue>>();
		}

		public async Task<RequestResult<EntityList<Category>>> GetCategories()
		{
			var request = RequestFactory.Create("oslc/categories.json");

			return await request.Execute<EntityList<Category>>();
		}

		public async Task<RequestResult<List<IssueState>>> GetStates(string projectUuid)
		{
			var request = RequestFactory.Create("oslc/workflows/{projectUuid}/states/com.ibm.team.workitem.taskWorkflow.json");

			request.AddUrlSegment("projectUuid", projectUuid);

			return await request.Execute<List<IssueState>>();
		}

		public async Task<RequestResult<List<WorkItemType>>> GetTypes(string projectUuid)
		{
			var request = RequestFactory.Create("oslc/types/{projectUuid}.json");

			request.AddUrlSegment("projectUuid", projectUuid);

			return await request.Execute<List<WorkItemType>>();
		}

		public async Task<RequestResult<Issue>> Update(long issueId, ChangeWorkItem changeWorkItem)
		{
			if (changeWorkItem == null) throw new ArgumentNullException(nameof(changeWorkItem));

			var request = RequestFactory.Create("/oslc/workitems/{issueId}", HttpMethod.Put);

			request.AddUrlSegment("issueId", issueId);
			request.SetDataFormat(DataFormat.Json);

			var createIssue = new ChangeIssue
			{
				Description = changeWorkItem.Description,
				Title = changeWorkItem.Title
			};

			if (!string.IsNullOrEmpty(changeWorkItem.StateResource))
				createIssue.State = new ResourceLink
				{
					Resource = changeWorkItem.StateResource
				};

			request.AddBody(createIssue);

			return await request.Execute<Issue>();
		}
	}
}