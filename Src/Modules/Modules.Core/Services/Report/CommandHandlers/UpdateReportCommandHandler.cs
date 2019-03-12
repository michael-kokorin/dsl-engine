namespace Modules.Core.Services.Report.CommandHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Infrastructure.Reports;
	using Modules.Core.Services.Report.Commands;

	[UsedImplicitly]
	internal sealed class UpdateReportCommandHandler : ICommandHandler<UpdateReportCommand>
	{
		private readonly IReportStorage _reportStorage;

		public UpdateReportCommandHandler([NotNull] IReportStorage reportStorage)
		{
			if (reportStorage == null) throw new ArgumentNullException(nameof(reportStorage));

			_reportStorage = reportStorage;
		}

		/// <summary>
		///   Processes the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		public void Process([NotNull] UpdateReportCommand command)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			if (command.Report.Id == null)
			{
				_reportStorage.Add(command.Report.ProjectId,
					command.Report.Name,
					command.Report.Description,
					command.Report.Rule);
			}
			else
			{
				_reportStorage.Update(command.Report.Id.Value,
					command.Report.Name,
					command.Report.Description,
					command.Report.Rule);
			}
		}
	}
}