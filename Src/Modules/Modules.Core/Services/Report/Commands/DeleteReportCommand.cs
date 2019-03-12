namespace Modules.Core.Services.Report.Commands
{
	using Common.Command;

	public sealed class DeleteReportCommand : ICommand
	{
		public readonly long ReportId;

		public DeleteReportCommand(long reportId)
		{
			ReportId = reportId;
		}
	}
}