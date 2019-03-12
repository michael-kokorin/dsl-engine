namespace Modules.Core.Services.Report.Commands
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Modules.Core.Contracts.Report.Dto;

	internal sealed class UpdateReportCommand : ICommand
	{
		public readonly ReportDto Report;

		public UpdateReportCommand([NotNull] ReportDto report)
		{
			if (report == null) throw new ArgumentNullException(nameof(report));

			Report = report;
		}
	}
}