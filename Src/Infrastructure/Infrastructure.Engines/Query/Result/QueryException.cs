namespace Infrastructure.Engines.Query.Result
{
	public sealed class QueryException
	{
		public string Message { get; set; }

		public string StackTrace { get; set; }
	}
}