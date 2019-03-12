namespace Modules.Core.Contracts.Transport
{
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Dispatcher;
	using System.Web;

	internal sealed class LocaleMessageInspector : IClientMessageInspector
	{
		private const string HeaderName = "Accept-Language";

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			HttpRequestMessageProperty httpRequestMessage;

			object httpRequestMessageObject;

			if (HttpContext.Current?.Request.Headers == null)
				return null;

			if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
			{
				httpRequestMessage = httpRequestMessageObject as HttpRequestMessageProperty;

				if (httpRequestMessage == null)
					return null;

				if (string.IsNullOrEmpty(httpRequestMessage.Headers[HeaderName]))
				{
					httpRequestMessage.Headers[HeaderName] = HttpContext.Current.Request.Headers[HeaderName];
				}
			}
			else
			{
				httpRequestMessage = new HttpRequestMessageProperty();
				httpRequestMessage.Headers.Add(HeaderName, HttpContext.Current.Request.Headers[HeaderName]);
				request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessage);
			}

			return null;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{

		}
	}
}