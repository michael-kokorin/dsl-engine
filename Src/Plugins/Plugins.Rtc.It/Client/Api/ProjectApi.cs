namespace Plugins.Rtc.It.Client.Api
{
	using System;
	using System.Threading.Tasks;

	using Plugins.Rtc.It.Client.Entity;

	public sealed class ProjectApi : BaseApi
	{
		internal ProjectApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<EntityList<Project>>> Get()
		{
			var request = RequestFactory.Create("oslc/projectareas.json");

			return await request.Execute<EntityList<Project>>();
		}

		public async Task<RequestResult<Project>> Get(long projectUuid)
		{
			if (projectUuid <= 0) throw new ArgumentOutOfRangeException(nameof(projectUuid));

			var request = RequestFactory.Create("oslc/projectareas/{projectUuid}.json");

			request.AddUrlSegment("projectUuid", projectUuid);

			return await request.Execute<Project>();
		}
	}
}