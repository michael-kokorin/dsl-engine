namespace Plugins.Rtc.It.Client
{
	using System;

	using RestSharp;

	internal sealed class RequestFactory : IRequestFactory
	{
		private readonly IClientFactory _clientFactory;

		public RequestFactory(IClientFactory clientFactory)
		{
			if (clientFactory == null) throw new ArgumentNullException(nameof(clientFactory));

			_clientFactory = clientFactory;
		}

		public IRequest Create(string resource, HttpMethod method = HttpMethod.Get)
		{
			var client = _clientFactory.GetOrCreate();

			return CreateRequest(client, resource, method);
		}

		private static IRequest CreateRequest(IRestClient client, string resource, HttpMethod method)
		{
			switch (method)
			{
				case HttpMethod.Get:
					return new Requiest(client, resource);
				case HttpMethod.Post:
					return new Requiest(client, resource, Method.POST);
				case HttpMethod.Put:
					return new Requiest(client, resource, Method.PUT);
				default:
					throw new Exception($"Unsupported HTTP method. Method='{method}'");
			}
		}
	}
}