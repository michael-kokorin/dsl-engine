namespace Plugins.GitLab.Client.Api
{
	using System;

	public abstract class BaseApi
	{
		protected readonly IRequestFactory RequestFactory;

		internal BaseApi(IRequestFactory requestFactory)
		{
			if (requestFactory == null) throw new ArgumentNullException(nameof(requestFactory));

			RequestFactory = requestFactory;
		}
	}
}
