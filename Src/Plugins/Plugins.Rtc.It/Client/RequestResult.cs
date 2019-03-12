namespace Plugins.Rtc.It.Client
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;

	using RestSharp;

	public abstract class RequestResult
	{
		public readonly Uri ResponseUri;

		public readonly HttpStatusCode StatusCode;

		public readonly string ErrorMessage;

		public readonly IEnumerable<Header> Headers;

		protected RequestResult(IRestResponse response)
		{
			if (response == null) throw new ArgumentNullException(nameof(response));

			ResponseUri = response.ResponseUri;
			StatusCode = response.StatusCode;
			ErrorMessage = response.ErrorMessage;

			Headers = response.Headers.Select(_ => new Header(_.Name, _.Value));
		}
	}

	public sealed class Header
	{
		public readonly string Key;

		public readonly object Value;

		public Header(string key, object value)
		{
			if (key == null) throw new ArgumentNullException(nameof(key));

			Key = key;
			Value = value;
		}
	}

	public sealed class RequestResult<T> : RequestResult
	{
		public readonly T Data;

		public RequestResult(IRestResponse<T> response)
			: base(response)
		{
			Data = response.Data;
		}

		public RequestResult(IRestResponse response, T data)
			: base(response)
		{
			if (response == null) throw new ArgumentNullException(nameof(response));

			Data = data;
		}
	}
}