namespace Infrastructure.Reports.Generation
{
	using System;

	internal sealed class IncorrectReportRuleException : Exception
	{
		public IncorrectReportRuleException(Exception ex)
			: base("Failed to deserialize report rule.", ex)
		{

		}
	}
}