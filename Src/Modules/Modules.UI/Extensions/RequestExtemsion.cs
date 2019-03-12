namespace Modules.UI.Extensions
{
	using System.Diagnostics;
	using System.Net.NetworkInformation;
	using System.Web;

	internal static class RequestExtemsion
	{
		public static string GetBaseUrl(this HttpRequest request)
		{
			var appUrl = HttpRuntime.AppDomainAppVirtualPath;

			if(!string.IsNullOrWhiteSpace(appUrl) &&
			   (appUrl.Length > 1))
				appUrl += "/";

			var host = GetMachineName();

			if(IsIisExpress())
				host = request.Url.Host;

			var baseUrl = $"{request.Url.Scheme}://{host}:{request.Url.Port}{appUrl}";

			return baseUrl;
		}

		private static bool IsIisExpress()
			=> string.CompareOrdinal(Process.GetCurrentProcess().ProcessName, "iisexpress") == 0;

		private static string GetMachineName()
		{
			var ipProperties = IPGlobalProperties.GetIPGlobalProperties();

			return $"{ipProperties.HostName}.{ipProperties.DomainName}".ToLower();
		}
	}
}