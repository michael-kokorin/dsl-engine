namespace Plugins.GitLab.Client
{
	using System;
	using System.Net;
	using System.Threading.Tasks;

	using Plugins.GitLab.Client.Api;

	using RestSharp;
	using RestSharp.Authenticators;

	public sealed class GitLabClient
	{
		private const string ApiPath = "/api/v3";

		public readonly BranchesApi Branches;

		public readonly CommitsApi Commits;

		public readonly FileApi Files;

		public readonly IssueApi Issues;

		public readonly ProjectApi Projects;

		public readonly UserApi Users;

		// ReSharper disable once UnusedMember.Local
		private GitLabClient()
		{

		}

		private GitLabClient(Uri uri, string privateToken)
		{
			var clientFactory = new ClientFactory(uri, new PrivateTokenAuthenticator(privateToken));

			var requestFactory = new RequestFactory(clientFactory);

			Branches = new BranchesApi(requestFactory);
			Commits = new CommitsApi(requestFactory);
			Files = new FileApi(requestFactory);
			Issues = new IssueApi(requestFactory);
			Projects = new ProjectApi(requestFactory);
			Users = new UserApi(requestFactory);
		}

		public static GitLabClient Connect(string host, string privateToken)
		{
			var hostUri = new Uri(host);

			var apiUri = new Uri(hostUri, ApiPath);

			return new GitLabClient(apiUri, privateToken);
		}
	}

	internal interface IClientFactory
	{
		IRestClient Create();
	}

	internal sealed class ClientFactory : IClientFactory
	{
		private readonly Uri _uri;

		private readonly IAuthenticator _authenticator;

		public ClientFactory(Uri uri, IAuthenticator authenticator)
		{
			_uri = uri;

			_authenticator = authenticator;
		}

		public IRestClient Create()
		{
			var client = new RestClient(_uri) { Authenticator = _authenticator };

			return client;
		}
	}

	internal sealed class PrivateTokenAuthenticator : IAuthenticator
	{
		private readonly string _privateToken;

		public PrivateTokenAuthenticator(string privateToken)
		{
			_privateToken = privateToken;
		}

		public void Authenticate(IRestClient client, IRestRequest request)
			=> request.AddHeader("PRIVATE-TOKEN", _privateToken);
	}

	public enum HttpMethod
	{
		/// <summary>
		/// HTTP GET method
		/// </summary>
		Get,

		/// <summary>
		/// HTTP POST method
		/// </summary>
		Post,

		/// <summary>
		/// HTTP PUT method
		/// </summary>
		Put
	}

	public interface IRequestFactory
	{
		IRequest Create(string resource, HttpMethod method = HttpMethod.Get);
	}

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
			var client = _clientFactory.Create();

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

	public interface IRequest
	{
		void AddUrlSegment(string key, object value);

		void AddParameter(string key, object valye);

		void AddParameterIfNotNull(string key, object value);

		Task<RequestResult<T>> Execute<T>() where T : new();

		Task<RequestResult<byte[]>> ExecuteBytes();
	}

	internal sealed class Requiest : IRequest
	{
		private readonly IRestClient _restClient;

		private readonly IRestRequest _restRequest;

		public Requiest(IRestClient restClient, string resource, Method method = Method.GET)
		{
			if (restClient == null) throw new ArgumentNullException(nameof(restClient));

			_restClient = restClient;

			_restRequest = new RestRequest(resource, method);
		}

		public void AddUrlSegment(string key, object value) => _restRequest.AddParameter(key, value, ParameterType.UrlSegment);

		public void AddParameter(string key, object valye) => _restRequest.AddParameter(key, valye);

		public void AddParameterIfNotNull(string key, object value)
		{
			if (value != null) AddParameter(key, value);
		}

		public async Task<RequestResult<T>> Execute<T>() where T : new()
		{
			var result = await _restClient.ExecuteTaskAsync<T>(_restRequest);

			return new RequestResult<T>(result);
		}

		public async Task<RequestResult<byte[]>> ExecuteBytes()
		{
			var result = await _restClient.ExecuteTaskAsync(_restRequest);

			return new RequestResult<byte[]>(result, result.RawBytes);
		}
	}

	public sealed class RequestResult<T>
	{
		public readonly HttpStatusCode StatusCode;

		public readonly T Data;

		public RequestResult(IRestResponse<T> response)
		{
			StatusCode = response.StatusCode;

			Data = response.Data;
		}

		public RequestResult(IRestResponse response, T data)
		{
			if (response == null)
				throw new ArgumentNullException(nameof(response));

			StatusCode = response.StatusCode;
			Data = data;
		}
	}
}