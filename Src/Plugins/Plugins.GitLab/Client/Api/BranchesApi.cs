namespace Plugins.GitLab.Client.Api
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Plugins.GitLab.Client.Entity;

	public sealed class BranchesApi : BaseApi
	{
		public BranchesApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<Branch>> Create(long projectId, string name, string source)
		{
			var request = RequestFactory.Create("projects/{projectId}/repository/branches", HttpMethod.Post);

			request.AddUrlSegment("projectId", projectId);
			request.AddParameter("branch_name", name);
			request.AddParameter("ref", source);

			return await request.Execute<Branch>();
		}

		public async Task<RequestResult<List<Branch>>> Get(long projectId)
		{
			var request = RequestFactory.Create("projects/{projectId}/repository/branches");

			request.AddUrlSegment("projectId", projectId);

			return await request.Execute<List<Branch>>();
		}

		public async Task<RequestResult<Branch>> Get(long projectId, string branchName)
		{
			var request = RequestFactory.Create("projects/{projectId}/repository/branches/{branchName}");

			request.AddUrlSegment("projectId", projectId);
			request.AddUrlSegment("branchName", branchName);

			return await request.Execute<Branch>();
		}

		public async Task<RequestResult<byte[]>> GetArchive(long projectId, string sha)
		{
			var request = RequestFactory.Create("projects/{projectId}/repository/archive.zip");

			request.AddUrlSegment("projectId", projectId);
			request.AddParameterIfNotNull("sha", sha);

			return await request.ExecuteBytes();
		}
	}
}