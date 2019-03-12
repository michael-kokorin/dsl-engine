namespace Infrastructure.Reports.Generation
{
	using System.Collections.Generic;

	using Repository.Context;

	public interface IReportGenerationPipelineManager
	{
		ReportBundle Generate(Reports report,
			long userId,
			IReadOnlyDictionary<string, object> parameters);
	}
}