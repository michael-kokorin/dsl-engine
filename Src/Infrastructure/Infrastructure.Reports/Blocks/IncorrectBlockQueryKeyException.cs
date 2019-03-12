namespace Infrastructure.Reports.Blocks
{
	using System;

	public sealed class IncorrectBlockQueryKeyException : Exception
	{
		public IncorrectBlockQueryKeyException(IQuaryableReportBlock block)
			: base($"Incorrect query key. QueryKey='{block.QueryKey}'")
		{
		}
	}
}