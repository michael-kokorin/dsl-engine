namespace Modules.Core.Services.Report.DataQueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Reports;
	using Modules.Core.Contracts.Report.Dto;
	using Modules.Core.Services.Report.DataQueries;

	[UsedImplicitly]
	internal sealed class GetReportQueryHandler : IDataQueryHandler<GetReportQuery, ReportDto>
	{
		private readonly IReportStorage _reportStorage;

		public GetReportQueryHandler([NotNull] IReportStorage reportStorage)
		{
			if (reportStorage == null) throw new ArgumentNullException(nameof(reportStorage));

			_reportStorage = reportStorage;
		}

		public ReportDto Execute([NotNull] GetReportQuery query)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));

			var report = _reportStorage.Get(query.ReportId);

			return new ReportDto
			{
				Description = report.Description,
				Id = report.Id,
				Name = report.DisplayName,
				ProjectId = report.ProjectId,
				Rule = report.Rule
			};
		}
	}
}