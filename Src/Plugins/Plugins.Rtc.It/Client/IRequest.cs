namespace Plugins.Rtc.It.Client
{
	using System.Threading.Tasks;

	public interface IRequest
	{
		void AddBody(object body);

		void AddHeader(string key, string value);

		void AddUrlSegment(string key, object value);

		void AddParameter(string key, object valye);

		void AddParameterIfNotNull(string key, object value);

		string Execute();

		Task<RequestResult<T>> Execute<T>() where T : new();

		Task<RequestResult<byte[]>> ExecuteBytes();

		void SetDataFormat(DataFormat dataFormat);
	}
}