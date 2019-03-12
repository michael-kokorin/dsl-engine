namespace Plugins.Rtc.It.Tests
{
	using System;
	using System.Linq;
	using System.Net;

	using NUnit.Framework;

	using RestSharp;

	[TestFixture]
	[Ignore]
	public sealed class RtcAuthorizationTest
	{
		[Test]
		public void Test()
		{
		 var cookieContainer = new CookieContainer();

			var client = new RestClient("https://10.5.208.17:9443/ccm") {CookieContainer = cookieContainer};

			ServicePointManager.ServerCertificateValidationCallback +=
				(sender, certificate, chain, sslPolicyErrors) => true;

			var restRequest = new RestRequest("authenticated/identity");

			var result = client.Execute(restRequest);

			if (result.StatusCode == HttpStatusCode.OK)
			{
				var header = GetHeader(result);

				if (header != null && header.Value.Equals("authrequired"))
				{
					var authRequest = new RestRequest("authenticated/j_security_check", Method.POST);

					authRequest.AddParameter("j_username", "msharonov");
					authRequest.AddParameter("j_password", "P@ssw0rd");

					//authRequest.AddBody("j_username=msharonov&amp;j_password=P@ssw0rd");

					var authResult = client.Execute(authRequest);

					var authHeader = GetHeader(authResult);

					if (authHeader != null && authHeader.Value.Equals("authfailed"))
					{

					}
					else
					{
						var workItemRequest = new RestRequest("oslc/workitems/1.json");

						result = client.Execute(workItemRequest);
					}
				}
			}
			else
			{
				throw new Exception("Incorrect Identity headers");
			}
		}

		private static Parameter GetHeader(IRestResponse result) =>
			result.Headers.FirstOrDefault(_ => _.Name.Equals(
				"x-com-ibm-team-repository-web-auth-msg",
				StringComparison.InvariantCultureIgnoreCase));
	}
}