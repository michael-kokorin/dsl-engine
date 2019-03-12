namespace Plugins.Rtc.It.Client.Api
{
	using System.Threading.Tasks;

	using Plugins.Rtc.It.Client.Entity;

	public sealed class AuthApi : BaseApi
	{
		public AuthApi(IRequestFactory requestFactory) : base(requestFactory)
		{
		}

		public async Task<RequestResult<Identity>> Identity()
		{
			var request = RequestFactory.Create("authenticated/identity");

			return await request.Execute<Identity>();
		}

		public async Task<RequestResult<SecurityCheck>> SecurityCheck(string username, string password)
		{
			var authRequest = RequestFactory.Create("authenticated/j_security_check", HttpMethod.Post);

			authRequest.AddParameter("j_username", username);
			authRequest.AddParameter("j_password", password);

			return await authRequest.Execute<SecurityCheck>();
		}
	}
}