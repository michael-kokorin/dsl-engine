namespace Infrastructure.Reports
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Security;
	using Common.Time;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class ReportStorage: IReportStorage
	{
		private readonly IReportAuthorityValidator _reportAuthorityValidator;

		private readonly IReportRepository _reportRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITimeService _timeService;

		private readonly IUserPrincipal _userPrincipal;

		public ReportStorage(
			[NotNull] IReportAuthorityValidator reportAuthorityValidator,
			[NotNull] IReportRepository reportRepository,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider,
			[NotNull] ITimeService timeService,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (reportAuthorityValidator == null) throw new ArgumentNullException(nameof(reportAuthorityValidator));
			if (reportRepository == null) throw new ArgumentNullException(nameof(reportRepository));
			if (telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));
			if (timeService == null) throw new ArgumentNullException(nameof(timeService));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_reportAuthorityValidator = reportAuthorityValidator;
			_reportRepository = reportRepository;
			_telemetryScopeProvider = telemetryScopeProvider;
			_timeService = timeService;
			_userPrincipal = userPrincipal;
		}

		public long Add(long? projectId, string name, string desc, string rule, bool isSystem)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (string.IsNullOrEmpty(rule))
				throw new ArgumentNullException(nameof(rule));

			using (var telemetryScope = _telemetryScopeProvider.Create<Reports>(TelemetryOperationNames.Report.Create))
			{
				try
				{
					if (_reportRepository.Get(projectId, name).Any())
					{
						throw new ReportAlreadyExistsException(name);
					}

					if (!_reportAuthorityValidator.CanCreate(_userPrincipal.Info.Id, projectId))
						throw new UnauthorizedAccessException();

					var report = new Reports
					{
						DisplayName = name,
						Created = _timeService.GetUtc(),
						CreatedById = _userPrincipal.Info.Id,
						Description = desc,
						Modified = _timeService.GetUtc(),
						ModifiedById = _userPrincipal.Info.Id,
						ProjectId = projectId,
						Rule = rule,
						IsSystem = isSystem
					};

					telemetryScope.SetEntity(report);

					_reportRepository.Insert(report);

					_reportRepository.Save();

					telemetryScope.WriteSuccess();

					return report.Id;
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		public void Delete(long reportId)
		{
			using (var telemetryScope = _telemetryScopeProvider.Create<Reports>(TelemetryOperationNames.Report.Delete))
			{
				try
				{
					var report = GetReport(reportId);

					telemetryScope.SetEntity(report);

					if (!_reportAuthorityValidator.CanEdit(_userPrincipal.Info.Id, report))
						throw new UnauthorizedAccessException();

					_reportRepository.Delete(report);

					_reportRepository.Save();

					telemetryScope.WriteSuccess();
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		public IQueryable<Reports> GetUserQuery()
		{
			var userId = _userPrincipal.Info.Id;

			var projectIds = _reportAuthorityValidator.GetProjects(userId, ReportAccessType.View);

			var projectReports = _reportRepository.Get(projectIds);

			var userReportIds = new List<long>();

			foreach (var projectReport in projectReports)
			{
				if (_reportAuthorityValidator.CanView(userId, projectReport))
					userReportIds.Add(projectReport.Id);
			}

			return _reportRepository.Query()
				.Where(_ => userReportIds.Contains(_.Id));
		}

		public Reports Get(long reportId)
		{
			var report = GetReport(reportId);

			if (!_reportAuthorityValidator.CanView(_userPrincipal.Info.Id, report))
				throw new UnauthorizedAccessException();

			if (!string.IsNullOrEmpty(report.Rule)) return report;

			var reportRule = new ReportRule
			{
				Parameters = new ReportParameter[0],
				QueryLinks = new IReportQuery[0]
			};

			report.Rule = reportRule.ToJson(false);

			_reportRepository.Save();

			return report;
		}

		public Reports Update(long reportId, [NotNull] string name, string desc, string rule)
		{
			using (var telemetryScope = _telemetryScopeProvider.Create<Reports>(TelemetryOperationNames.Report.Update))
			{
				try
				{
					var report = GetReport(reportId);

					telemetryScope.SetEntity(report);

					if (!_reportAuthorityValidator.CanEdit(_userPrincipal.Info.Id, report))
						throw new UnauthorizedAccessException();

					report.Description = desc;
					report.DisplayName = name;
					report.Rule = rule;
					report.Modified = _timeService.GetUtc();
					report.ModifiedById = _userPrincipal.Info.Id;

					_reportRepository.Save();

					telemetryScope.WriteSuccess();

					return report;
				}
				catch (Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		private Reports GetReport(long reportId)
		{
			var report = _reportRepository.GetById(reportId);

			if (report == null)
				throw new ReportDoesNotExistsException(reportId);

			return report;
		}
	}
}