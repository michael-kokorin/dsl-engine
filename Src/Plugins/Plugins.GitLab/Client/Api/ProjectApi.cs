namespace Plugins.GitLab.Client.Api
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	using Plugins.GitLab.Client;
	using Plugins.GitLab.Client.Entity;

	public sealed class ProjectApi : BaseApi
	{
		internal ProjectApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<List<Project>>> Accessible()
		{
			var request = RequestFactory.Create("projects");

			return await request.Execute<List<Project>>();
		}

		public async Task<RequestResult<List<Project>>> Owned()
		{
			var request = RequestFactory.Create("projects/owned");

			return await request.Execute<List<Project>>();
		}
	}
}