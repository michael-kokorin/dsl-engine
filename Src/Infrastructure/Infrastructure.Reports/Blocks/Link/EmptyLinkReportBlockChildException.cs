namespace Infrastructure.Reports.Blocks.Link
{
	using System;

	internal sealed class EmptyLinkReportBlockChildException : Exception
	{
		public EmptyLinkReportBlockChildException(string blockId)
			: base($"Empty link report block child. BlockId='{blockId}'")
		{

		}
	}
}