namespace Infrastructure.Reports
{
	using System;

	internal sealed class ReportDoesNotExistsException : Exception
	{
		internal ReportDoesNotExistsException(long reportId)
			: base($"Report does not exists. ReportId='{reportId}'")
		{

		}
	}
}