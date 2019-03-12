namespace Infrastructure.Reports.Blocks
{
	using System;

	internal sealed class QueryResultNotFoundException : Exception
	{
		public QueryResultNotFoundException(IQuaryableReportBlock block)
			: base($"Query result not found. QueryKey='{block.QueryKey}'")
		{

		}
	}
}