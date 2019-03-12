namespace Plugins.Rtc.It.Client
{
	using RestSharp;

	internal interface IClientFactory
	{
		IRestClient Create();

		IRestClient GetOrCreate();
	}
}