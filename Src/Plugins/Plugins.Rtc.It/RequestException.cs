namespace Plugins.Rtc.It
{
	using System;

	using Plugins.Rtc.It.Client;

	internal sealed class RequestException : Exception
	{
		public RequestException(RequestResult requestResult)
			: base(
				$"REST method execution failed. Uri='{requestResult.ResponseUri.AbsoluteUri}', StatusCode='{requestResult.StatusCode}', Message='{requestResult.ErrorMessage}'"
				)
		{

		}
	}
}