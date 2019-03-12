namespace Modules.Core.Services.Report.DataQueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Query;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Reports;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.Report.DataQueries;
	using Modules.Core.Services.UI.Renderers;

	[UsedImplicitly]
	internal sealed class GetReportsListQueryHandler : IDataQueryHandler<GetReportsListQuery, TableDto>
	{
		private readonly IReportStorage _reportStorage;

		public GetReportsListQueryHandler([NotNull] IReportStorage reportStorage)
		{
			if (reportStorage == null) throw new ArgumentNullException(nameof(reportStorage));

			_reportStorage = reportStorage;
		}

		public TableDto Execute([NotNull] GetReportsListQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var query = _reportStorage.GetUserQuery()
				.Select(_ => new
				{
					_.Id,
					Project = _.Projects.DisplayName,
					_.DisplayName,
					_.Created,
					CreatedBy = _.Users.DisplayName,
					_.Modified,
					ModifiedBy = _.Users1.DisplayName,
					_.IsSystem
				})
				.ToArray()
				.Select(_ => new QueryResultItem
				{
					EntityId = _.Id,
					Value = new
					{
						_.Id,
						_.Project,
						_.DisplayName,
						Created = _.Created.ToString("g"),
						_.CreatedBy,
						Modified = _.Modified.ToString("g"),
						_.ModifiedBy,
						_.IsSystem
					}
				});

			return new TableRenderer().Render(query);
		}
	}
}