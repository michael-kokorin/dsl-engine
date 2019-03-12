namespace Modules.Core.Services.Query.DataQueries
{
	using System;

	using Common.Query;

	public sealed class GetQueryModelQuery : IDataQuery
	{
		public readonly string Query;

		public GetQueryModelQuery(string query)
		{
			if (string.IsNullOrEmpty(query))
				throw new ArgumentNullException(nameof(query));

			Query = query;
		}
	}
}