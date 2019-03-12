namespace Modules.Core.Tests.Services.Report.CommandHandlers
{
	using Moq;

	using NUnit.Framework;

	using Common.Command;
	using Infrastructure.Reports;
	using Modules.Core.Services.Report.CommandHandlers;
	using Modules.Core.Services.Report.Commands;

	[TestFixture]
	public sealed class DeleteReportCommandHandlerTest
	{
		private ICommandHandler<DeleteReportCommand> _target;

		private Mock<IReportStorage> _reportStorage;

		[SetUp]
		public void SetUp()
		{
			_reportStorage = new Mock<IReportStorage>();

			_target = new DeleteReportCommandHandler(_reportStorage.Object);
		}

		[Test]
		public void ShouldProcessDeleteReportCommand()
		{
			const long reportId = 234;

			_target.Process(new DeleteReportCommand(reportId));

			_reportStorage.Verify(_ => _.Delete(reportId), Times.Once);
		}
	}
}