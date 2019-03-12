namespace Plugins.Rtc.It.Client
{
	using System;
	using System.Threading.Tasks;

	using RestSharp;

	internal sealed class Requiest : IRequest
	{
		private readonly IRestClient _restClient;

		private readonly IRestRequest _restRequest;

		public Requiest(IRestClient restClient, string resource, Method method = Method.GET)
		{
			if (restClient == null) throw new ArgumentNullException(nameof(restClient));

			_restClient = restClient;

			_restRequest = new RestRequest(resource, method)
			{
				JsonSerializer = new RestSharpJsonNetSerializer()
			};
		}

		public void AddUrlSegment(string key, object value) => _restRequest.AddParameter(key, value, ParameterType.UrlSegment);

		public void AddParameter(string key, object valye) => _restRequest.AddParameter(key, valye);

		public void AddBody(object body) => _restRequest.AddBody(body);

		public void AddHeader(string key, string value) => _restRequest.AddParameter(key, value, ParameterType.RequestBody);

		public void AddParameterIfNotNull(string key, object value)
		{
			if (value != null) AddParameter(key, value);
		}

		public string Execute()
		{
			var result = _restClient.Execute(_restRequest).Content;

			return result;
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

		public void SetDataFormat(DataFormat dataFormat)
		{
			switch (dataFormat)
			{
				case DataFormat.Json:
					_restRequest.RequestFormat = RestSharp.DataFormat.Json;
					break;
				case DataFormat.Xml:
					_restRequest.RequestFormat = RestSharp.DataFormat.Xml;
					break;
				default:
					throw new Exception();
			}
		}
	}
}