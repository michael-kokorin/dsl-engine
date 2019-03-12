namespace Plugins.GitLab.Client.Api
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Plugins.GitLab.Client.Entity;

	public sealed class CommitsApi : BaseApi
	{
		internal CommitsApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<List<Commit>>> Get(long projectId, DateTime? since, DateTime? until)
		{
			var request = RequestFactory.Create("projects/{projectId}/repository/commits");

			request.AddUrlSegment("projectId", projectId);
			request.AddParameterIfNotNull("since", since);
			request.AddParameterIfNotNull("until", until);

			return await request.Execute<List<Commit>>();
		}
	}
}