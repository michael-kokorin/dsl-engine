namespace Plugins.GitLab.Client.Api
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Issue = Plugins.GitLab.Client.Entity.Issue;

	public sealed class IssueApi : BaseApi
	{
		internal IssueApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<Issue>> Create(long projectId, CreateIssue createIssue)
		{
			var request = RequestFactory.Create("projects/{projectId}/issues", HttpMethod.Post);

			request.AddUrlSegment("projectId", projectId);
			request.AddParameterIfNotNull("title", createIssue.Title);
			request.AddParameterIfNotNull("description", createIssue.Description);

			return await request.Execute<Issue>();
		}

		public async Task<RequestResult<List<Issue>>> Get(long projectId, GetIssue getIssue)
		{
			if (getIssue == null) throw new ArgumentNullException(nameof(getIssue));

			var request = RequestFactory.Create("projects/{projectId}/issues");

			request.AddUrlSegment("projectId", projectId);
			request.AddParameterIfNotNull("iid", getIssue.Iid);

			return await request.Execute<List<Issue>>();
		}

		public async Task<RequestResult<Issue>> Update(long projectId, long issueId, UpdateIssue updateIssue)
		{
			var request = RequestFactory.Create("projects/{projectId}/issues/{issueId}", HttpMethod.Put);

			request.AddUrlSegment("projectId", projectId);
			request.AddUrlSegment("issueId", issueId);

			request.AddParameterIfNotNull("title", updateIssue.Title);
			request.AddParameterIfNotNull("description", updateIssue.Description);
			request.AddParameterIfNotNull("state_event", updateIssue.State);

			return await request.Execute<Issue>();
		}
	}
}