namespace Infrastructure.Query
{
	using System;

	internal sealed class QueryDoesNotExistsException : Exception
	{
		public QueryDoesNotExistsException(long queryId)
			: base($"Query does not exists. Id='{queryId}'")
		{

		}

		public QueryDoesNotExistsException(string queryName)
			: base($"Query does not exitsts. Name='{queryName}'")
		{

		}
	}
}