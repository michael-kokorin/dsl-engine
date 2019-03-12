namespace Plugins.GitLab.Client.Api
{
	using System.Threading.Tasks;

	using Plugins.GitLab.Client.Entity;

	public sealed class UserApi : BaseApi
	{
		internal UserApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<User>> GetCurrent()
		{
			var request = RequestFactory.Create("user");

			return await request.Execute<User>();
		}
	}
}