namespace Infrastructure.Reports.Blocks.Link
{
	using System;

	internal sealed class EmptyLinkReportBlockTargetException: Exception
	{
		public EmptyLinkReportBlockTargetException(string blockId)
			: base($"Empty link report block target. BlockId='{blockId}'")
		{
		}
	}
}