namespace Infrastructure.Reports.Tests
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using Moq;

	using NUnit.Framework;

	using Common.Security;
	using Common.Time;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;

	[TestFixture]
	public sealed class ReportStorageTests
	{
		private IReportStorage _target;

		private Mock<IReportAuthorityValidator> _reportAuthorityValidator;

		private Mock<IReportRepository> _reportRepository;

		private Mock<ITelemetryScopeProvider> _telemetryScopeProvider;

		private Mock<ITimeService> _timeService;

		private Mock<IUserPrincipal> _userPrincipal;

		[SetUp]
		public void SetUp()
		{
			_reportAuthorityValidator = new Mock<IReportAuthorityValidator>();
			_reportRepository = new Mock<IReportRepository>();
			_telemetryScopeProvider = new Mock<ITelemetryScopeProvider>();
			_timeService = new Mock<ITimeService>();
			_userPrincipal = new Mock<IUserPrincipal>();

			_telemetryScopeProvider
				.Setup(_ => _.Create<Reports>(It.IsAny<string>()))
				.Returns(Mock.Of<ITelemetryScope<Reports>>());

			_target = new ReportStorage(
				_reportAuthorityValidator.Object,
				_reportRepository.Object,
				_telemetryScopeProvider.Object,
				_timeService.Object,
				_userPrincipal.Object);
		}

		[Test]
		public void ShouldCreateRport()
		{
			const long projectId = 213;
			const string name = "report name";
			const string desc = "report desc";
			const string rule = "repo rule";
			const bool isSystem = true;

			const long userId = 12342;

			_reportRepository
				.Setup(_ => _.Get(projectId, name))
				.Returns(Enumerable.Empty<Reports>().AsQueryable());

			_reportAuthorityValidator
				.Setup(_ => _.CanCreate(userId, projectId))
				.Returns(true);

			_userPrincipal
				.Setup(_ => _.Info)
				.Returns(new UserInfo
				{
					Id = userId
				});

			_target.Add(projectId, name, desc, rule, isSystem);

			_reportRepository
				.Verify(_ => _.Insert(It.Is<Reports>(r =>
					r.CreatedById == userId &&
					r.DisplayName == name &&
					r.Description == desc &&
					r.ModifiedById == userId &&
					r.ProjectId == projectId &&
					r.Rule == rule
					)),
					Times.Once);

			_reportRepository
				.Verify(_ => _.Save(), Times.Once);
		}

		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		public void ShouldThrowWhenCantCreateReport()
		{
			const long projectId = 213;
			const string name = "report name";
			const string desc = "report desc";
			const string rule = "repo rule";
			const bool isSystem = true;

			const long userId = 12342;

			_reportRepository
				.Setup(_ => _.Get(projectId, name))
				.Returns(Enumerable.Empty<Reports>().AsQueryable());

			_userPrincipal
				.Setup(_ => _.Info)
				.Returns(new UserInfo
				{
					Id = userId
				});

			_reportAuthorityValidator
				.Setup(_ => _.CanCreate(userId, projectId))
				.Returns(false);

			_target.Add(projectId, name, desc, rule, isSystem);
		}

		[Test]
		public void ShouldDeleteReport()
		{
			const long reportId = 213;

			const long userId = 12342;

			var report = new Reports
			{
				Id = reportId
			};

			_reportRepository.Setup(_ => _.GetById(reportId)).Returns(report);

			_reportAuthorityValidator
				.Setup(_ => _.CanEdit(userId, report))
				.Returns(true);

			_userPrincipal
				.Setup(_ => _.Info)
				.Returns(new UserInfo
				{
					Id = userId
				});

			_target.Delete(reportId);

			_reportRepository.Verify(_ => _.Delete(report), Times.Once);

			_reportRepository.Verify(_ => _.Save(), Times.Once);
		}

		[Test]
		[ExpectedException(typeof(ReportDoesNotExistsException))]
		public void ShouldThrowWhenReportDoesntExistsOnDelete()
		{
			const long reportId = 213;

			_reportRepository.Setup(_ => _.GetById(reportId)).Returns((Reports) null);

			_target.Delete(reportId);
		}

		[Test]
		public void ShouldGetReport()
		{
			const long reportId = 5223434;

			var report = new Reports
			{
				Id = reportId
			};

			const long userId = 234;

			_reportRepository
				.Setup(_ => _.GetById(reportId))
				.Returns(report);

			_userPrincipal
				.Setup(_ => _.Info)
				.Returns(new UserInfo
				{
					Id = userId
				});

			_reportAuthorityValidator
				.Setup(_ => _.CanView(userId, report))
				.Returns(true);

			var result = _target.Get(reportId);

			result.ShouldBeEquivalentTo(report);
		}

		[Test]
		public void ShouldUpdateReport()
		{
			const long reportId = 5223434;

			var report = new Reports
			{
				Id = reportId,
				DisplayName = "repo name 1",
				Description = "repo desc 1",
				Rule = "repo rule desc",
				ModifiedById = 2
			};

			_reportRepository
				.Setup(_ => _.GetById(reportId))
				.Returns(report);

			const long userId = 234;

			_userPrincipal
				.Setup(_ => _.Info)
				.Returns(new UserInfo
				{
					Id = userId
				});

			_reportAuthorityValidator
				.Setup(_ => _.CanEdit(userId, report))
				.Returns(true);

			const string name2 = "repo name 2";
			const string desc2 = "repo desc 2";
			const string rule2 = "repo rule 2";

			var result = _target.Update(reportId, name2, desc2, rule2);

			result.DisplayName.ShouldBeEquivalentTo(name2);
			result.Description.ShouldBeEquivalentTo(desc2);
			result.Rule.ShouldBeEquivalentTo(rule2);
			result.ModifiedById.ShouldBeEquivalentTo(userId);
		}
	}
}