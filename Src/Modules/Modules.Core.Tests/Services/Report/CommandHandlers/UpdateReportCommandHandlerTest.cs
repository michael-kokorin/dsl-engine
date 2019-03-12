namespace Modules.Core.Tests.Services.Report.CommandHandlers
{
	using Moq;

	using NUnit.Framework;

	using Common.Command;
	using Infrastructure.Reports;
	using Modules.Core.Contracts.Report.Dto;
	using Modules.Core.Services.Report.CommandHandlers;
	using Modules.Core.Services.Report.Commands;

	[TestFixture]
	public sealed class UpdateReportCommandHandlerTest
	{
		private ICommandHandler<UpdateReportCommand> _target;

		private Mock<IReportStorage> _reportStorage;

		[SetUp]
		public void SetUp()
		{
			_reportStorage = new Mock<IReportStorage>();

			_target = new UpdateReportCommandHandler(_reportStorage.Object);
		}

		private static ReportDto GetReport(long? id) =>
			new ReportDto
			{
				Description = "desc",
				Id = id,
				Name = "repo",
				ProjectId = 123,
				Rule = "rule"
			};

		[Test]
		public void ShouldProcessUpdateReportCommandForNewReport()
		{
			var reportDto = GetReport(null);

			_target.Process(new UpdateReportCommand(reportDto));

			_reportStorage.Verify(_ => _.Add(
				reportDto.ProjectId,
				reportDto.Name,
				reportDto.Description,
				reportDto.Rule,
				false),
				Times.Once);
		}

		[Test]
		public void ShouldProcessUpdateReportCommandForExistsReport()
		{
			var reportDto = GetReport(123);

			_target.Process(new UpdateReportCommand(reportDto));

			_reportStorage.Verify(_ => _.Update(
				reportDto.Id.Value,
				reportDto.Name,
				reportDto.Description,
				reportDto.Rule),
				Times.Once);
		}
	}
}