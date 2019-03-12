namespace Plugins.Rtc.It.Client
{
	using System;

	using Plugins.Rtc.It.Client.Api;

	using RestSharp.Authenticators;

	internal sealed class RtcClient
	{
		public readonly AuthApi Auth;

		public readonly IssueApi Issues;

		public readonly ProjectApi Projects;

		private RtcClient(Uri uri, BasicAuthentication basicAuthentication = null)
		{
			ClientFactory clientFactory;

			if (basicAuthentication != null)
			{
				clientFactory = new ClientFactory(uri,
					new HttpBasicAuthenticator(
						basicAuthentication.Username,
						basicAuthentication.Password));
			}
			else
			{
				clientFactory = new ClientFactory(uri, null);
			}
			var requestFactory = new RequestFactory(clientFactory);

			Auth = new AuthApi(requestFactory);
			Issues = new IssueApi(requestFactory);
			Projects = new ProjectApi(requestFactory);
		}

		public static RtcClient Connect(string host, string ccmAppName, BasicAuthentication basicAuthentication)
		{
			var hostUri = new Uri(host);

			var apiUri = new Uri(hostUri, ccmAppName);

			return new RtcClient(apiUri, basicAuthentication);
		}
	}
}