namespace Infrastructure.Reports
{
	using System;

	public sealed class ReportRuleIsEmptyException : Exception
	{
		public ReportRuleIsEmptyException(long reportId)
			: base($"Report rule is empty. Rule Id='{reportId}'")
		{

		}
	}
}