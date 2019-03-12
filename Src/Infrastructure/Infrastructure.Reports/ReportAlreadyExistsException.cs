namespace Infrastructure.Reports
{
	using System;

	internal sealed class ReportAlreadyExistsException : Exception
	{
		public ReportAlreadyExistsException(string name)
			: base($"Report already exists. Name='{name}'")
		{

		}
	}
}