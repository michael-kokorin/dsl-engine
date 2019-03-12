namespace Plugins.Rtc.It.Client
{
	using System;
	using System.Net;

	using RestSharp;
	using RestSharp.Authenticators;

	internal sealed class ClientFactory : IClientFactory
	{
		private readonly Uri _uri;

		private readonly IAuthenticator _authenticator;

		private IRestClient _restClient;

		public ClientFactory(Uri uri, IAuthenticator authenticator)
		{
			_uri = uri;

			_authenticator = authenticator;
		}

		public IRestClient Create()
		{
			_restClient = new RestClient(_uri)
			{
				Authenticator = _authenticator,
				CookieContainer = new CookieContainer()
			};

			ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

			return _restClient;
		}

		public IRestClient GetOrCreate() => _restClient ?? Create();
	}
}