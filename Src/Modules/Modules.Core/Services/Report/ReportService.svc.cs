namespace Modules.Core.Services.Report
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Enums;
	using Common.Extensions;
	using Common.Logging;
	using Common.Query;
	using Modules.Core.Contracts.Report;
	using Modules.Core.Contracts.Report.Dto;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Services.Query.DataQueries;
	using Modules.Core.Services.Report.Commands;
	using Modules.Core.Services.Report.DataQueries;

	public sealed class ReportService : IReportService
	{
		private readonly IDataQueryDispatcher _dataQueryDispatcher;

		private readonly ICommandDispatcher _commandDispatcher;

		public ReportService([NotNull] IDataQueryDispatcher dataQueryDispatcher,
			[NotNull] ICommandDispatcher commandDispatcher)
		{
			if (dataQueryDispatcher == null) throw new ArgumentNullException(nameof(dataQueryDispatcher));
			if (commandDispatcher == null) throw new ArgumentNullException(nameof(commandDispatcher));

			_dataQueryDispatcher = dataQueryDispatcher;
			_commandDispatcher = commandDispatcher;
		}

		[LogMethod]
		public void Delete(long reportId)
		{
			var command = new DeleteReportCommand(reportId);

			_commandDispatcher.Handle(command);
		}

		[LogMethod(LogInputParameters = true)]
		public ReportFileDto Execute(long reportId, string parameters, int reportFileType)
		{
			IReadOnlyDictionary<string, string> queryParameters = null;

			if (!string.IsNullOrEmpty(parameters))
			{
				var temp = parameters.FromJson<QueryParameterValue[]>();

				if (temp != null)
				{
					queryParameters = temp.ToDictionary(_ => _.Key, _ => _.Value);
				}
			}

			var query = new BuildReportQuery(reportId, queryParameters, (ReportFileType) reportFileType);

			return _dataQueryDispatcher.Process<BuildReportQuery, ReportFileDto>(query);
		}

		[LogMethod(LogInputParameters = true)]
		public ReportDto Get(long reportId)
		{
			var query = new GetReportQuery(reportId);

			return _dataQueryDispatcher.Process<GetReportQuery, ReportDto>(query);
		}

		[LogMethod]
		public TableDto GetList() =>
			_dataQueryDispatcher.Process<GetReportsListQuery, TableDto>(new GetReportsListQuery());

		[LogMethod]
		public void Set(ReportDto report)
		{
			var command = new UpdateReportCommand(report);

			_commandDispatcher.Handle(command);
		}
	}
}