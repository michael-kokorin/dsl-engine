namespace Infrastructure.Reports.Generation.Stages
{
	using System;

	using JetBrains.Annotations;

	internal abstract class ReportGenerationStage : IReportGenerationStage
	{
		private IReportGenerationStage _successor;

		public void SetSuccessor([NotNull] IReportGenerationStage reportGenerationStage) =>
			_successor = reportGenerationStage;

		public void Process([NotNull] ReportBundle reportBundle)
		{
			if (reportBundle == null) throw new ArgumentNullException(nameof(reportBundle));

			ExecuteStage(reportBundle);

			_successor?.Process(reportBundle);
		}

		protected abstract void ExecuteStage(ReportBundle reportBundle);
	}
}