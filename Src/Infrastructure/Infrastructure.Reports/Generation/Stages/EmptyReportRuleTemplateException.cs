namespace Infrastructure.Reports.Generation.Stages
{
	using System;

	internal sealed class EmptyReportRuleTemplateException : Exception
	{
		public EmptyReportRuleTemplateException(ReportBundle reportBundle) :
			base($"Empty report template. Report Id='{reportBundle.Report.Id}'")
		{

		}
	}
}