namespace Infrastructure.Reports.Generation.Stages
{
	using System;

	internal sealed class UnknownReportGenerationStageException : Exception
	{
		public UnknownReportGenerationStageException(Type reportGenerationStage, Exception ex)
			: base($"Unknown report generation stage. Type='{reportGenerationStage.FullName}'", ex)
		{

		}
	}
}