namespace Plugins.Rtc.It.Client
{
	public interface IRequestFactory
	{
		IRequest Create(string resource, HttpMethod method = HttpMethod.Get);
	}
}