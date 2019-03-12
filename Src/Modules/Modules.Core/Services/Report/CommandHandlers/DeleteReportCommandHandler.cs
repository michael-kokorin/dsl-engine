namespace Modules.Core.Services.Report.CommandHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Infrastructure.Reports;
	using Modules.Core.Services.Report.Commands;

	[UsedImplicitly]
	public sealed class DeleteReportCommandHandler : ICommandHandler<DeleteReportCommand>
	{
		private readonly IReportStorage _reportStorage;

		public DeleteReportCommandHandler([NotNull] IReportStorage reportStorage)
		{
			if (reportStorage == null) throw new ArgumentNullException(nameof(reportStorage));

			_reportStorage = reportStorage;
		}

		/// <summary>
		///   Processes the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		public void Process([NotNull] DeleteReportCommand command)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			_reportStorage.Delete(command.ReportId);
		}
	}
}