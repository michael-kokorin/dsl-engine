namespace Modules.Core.Services.Report.DataQueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Infrastructure.Reports;
	using Modules.Core.Contracts.Report.Dto;
	using Modules.Core.Services.Report.DataQueries;

	[UsedImplicitly]
	internal sealed class BuildReportQueryHandler : IDataQueryHandler<BuildReportQuery, ReportFileDto>
	{
		private readonly IReportBuilder _reportBuilder;

		private readonly IUserPrincipal _userPrincipal;

		public BuildReportQueryHandler(
			[NotNull] IReportBuilder reportBuilder,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (reportBuilder == null) throw new ArgumentNullException(nameof(reportBuilder));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_reportBuilder = reportBuilder;
			_userPrincipal = userPrincipal;
		}

		public ReportFileDto Execute([NotNull] BuildReportQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var userId = _userPrincipal.Info.Id;

			var result = _reportBuilder.Build(
				dataQuery.ReportId,
				userId,
				dataQuery.Parameters?.ToDictionary(_ => _.Key, _ => (object) _.Value),
				dataQuery.ReportFileType);

			if (result == null)
				return null;

			return new ReportFileDto
			{
				Content = result.Content,
				Title = result.FileName
			};
		}
	}
}