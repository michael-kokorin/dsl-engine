namespace Modules.Core.Services.UI.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Licencing;
	using Common.Licencing.Licences;
	using Common.Security;
	using Common.Time;
	using Common.Transaction;
	using Infrastructure.AD;
	using Infrastructure.Notifications;
	using Infrastructure.Plugins;
	using Infrastructure.Policy;
	using Infrastructure.Security;
	using Infrastructure.Telemetry;
	using Modules.Core.Environment;
	using Modules.Core.Services.UI.Commands;
	using Repository.Context;
	using Repository.Repositories;

	using Authorities = Common.Security.Authorities;

	[UsedImplicitly]
	internal sealed class CreateProjectCommandHandler: CommandHandler<CreateProjectCommand>
	{
		private readonly IAuthorityProvider _authorityProvider;

		private readonly ILicenceProvider _licenceProvider;

		private readonly INotificationRuleProvider _notificationRuleProvider;

		private readonly IPluginProvider _pluginProvider;

		private readonly IQueryRepository _queryRepository;

		private readonly IReportRepository _reportRepository;

		private readonly IProjectRepository _repositoryProjects;

		private readonly IRoleProvider _roleProvider;

		private readonly ISdlPolicyProvider _sdlPolicyProvider;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		private readonly ITimeService _timeService;

		private readonly IUserPrincipal _userPrincipal;

		public CreateProjectCommandHandler(
			[NotNull] IAuthorityProvider authorityProvider,
			[NotNull] INotificationRuleProvider notificationRuleProvider,
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IProjectRepository repositoryProjects,
			[NotNull] IRoleProvider roleProvider,
			[NotNull] ISdlPolicyProvider sdlPolicyProvider,
			[NotNull] ITimeService timeService,
			[NotNull] IUnitOfWork unitOfWork,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] IQueryRepository queryRepository,
			[NotNull] IReportRepository reportRepository,
			[NotNull] ILicenceProvider licenceProvider,
			[NotNull] IPluginProvider pluginProvider,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
			: base(userAuthorityValidator, unitOfWork, userPrincipal)
		{
			if(authorityProvider == null) throw new ArgumentNullException(nameof(authorityProvider));
			if(notificationRuleProvider == null) throw new ArgumentNullException(nameof(notificationRuleProvider));
			if(userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if(repositoryProjects == null) throw new ArgumentNullException(nameof(repositoryProjects));
			if(roleProvider == null) throw new ArgumentNullException(nameof(roleProvider));
			if(sdlPolicyProvider == null) throw new ArgumentNullException(nameof(sdlPolicyProvider));
			if(timeService == null) throw new ArgumentNullException(nameof(timeService));
			if(userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));
			if(queryRepository == null) throw new ArgumentNullException(nameof(queryRepository));
			if(reportRepository == null) throw new ArgumentNullException(nameof(reportRepository));
			if(licenceProvider == null) throw new ArgumentNullException(nameof(licenceProvider));
			if(pluginProvider == null) throw new ArgumentNullException(nameof(pluginProvider));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_authorityProvider = authorityProvider;
			_notificationRuleProvider = notificationRuleProvider;
			_repositoryProjects = repositoryProjects;
			_roleProvider = roleProvider;
			_sdlPolicyProvider = sdlPolicyProvider;
			_userPrincipal = userPrincipal;
			_queryRepository = queryRepository;
			_reportRepository = reportRepository;
			_licenceProvider = licenceProvider;
			_pluginProvider = pluginProvider;
			_telemetryScopeProvider = telemetryScopeProvider;
			_timeService = timeService;
		}

		protected override string RequestedAuthorityName => Authorities.UI.Project.ProjectsList.Create;

		private void AddNotificationRule(
				long projectId,
				string ruleName,
				string query) =>
			_notificationRuleProvider.Create(
				projectId,
				ruleName,
				query);

		private void CreateNotificationRules(long projectId)
		{
			var notifications = new List<NotificationBundle>
								{
									new NotificationBundle(
										UserType.Developer,
										EnvironmentConstants.DeveloperPolicyViolationRuleName,
										"Developer: policy violated"),
									new NotificationBundle(
										UserType.Developer,
										EnvironmentConstants.DeveloperPolicySuccessfulRuleName,
										"Developer: policy accomplished"),
									new NotificationBundle(
										UserType.Developer,
										EnvironmentConstants.DeveloperTaskFinishedRuleName,
										"Developer: scan finished"),
									new NotificationBundle(
										UserType.Manager,
										EnvironmentConstants.ManagerPolicySuccessfulRuleName,
										"Mgr: policy accomplished"),
									new NotificationBundle(
										UserType.Manager,
										EnvironmentConstants.ManagerPolicyViolationRuleName,
										"Mgr: policy violated"),
									new NotificationBundle(
										UserType.Manager,
										EnvironmentConstants.ManagerTaskFinishedRuleName,
										"Mgr: scan finished")
								};

			var currentLicence = _licenceProvider.GetCurrent();

			if(currentLicence.Id == LicenceIds.Ftp)
			{
				notifications.Add(
					new NotificationBundle(UserType.Developer, EnvironmentConstants.DeveloperFtpTechByTrigger, "Placeholder report"));
				notifications.Add(
					new NotificationBundle(
						UserType.Manager,
						EnvironmentConstants.ManagerFtpAnalystReportByTrigget,
						"Placeholder report"));
			}

			var pciDssReport = _reportRepository.Get(projectId, "Scan results - PCI DSS").SingleOrDefault();

			var ftpTechReport = _reportRepository.Get(projectId, "FTP technical report").SingleOrDefault();

			var devReportAttach = $"\nreport {ftpTechReport?.Id}\nformat Pdf";

			var ftpAnalystReport = _reportRepository.Get(projectId, "FTP analytical report").SingleOrDefault();

			var manReportAttach = $"\nreport {ftpAnalystReport?.Id}\nformat Pdf\nparameters VulnType:XSS;";

			if(pciDssReport == null)
				return;

			foreach(var notification in notifications)
			{
				var notificationQuery = new EnvironmentConstants().GetNotificationRule(notification.NotificationRuleName);

				var notificationReport = _reportRepository.Get(projectId, notification.NotificationReportName).SingleOrDefault();

				if(notificationReport == null) continue;

				notificationQuery = notificationQuery
					.Replace("$NotificationReport$", notificationReport.Id.ToString())
					.Replace("$PciDssReport$", pciDssReport.Id.ToString());

				if(currentLicence.Id == LicenceIds.Ftp)
				{
					switch(notification.UserType)
					{
						case UserType.Developer:
							notificationQuery += devReportAttach;
							break;
						case UserType.Manager:
							notificationQuery += manReportAttach;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				AddNotificationRule(projectId, notification.NotificationRuleName, notificationQuery);
			}
		}

		private void CreatePolicyRules(long projectId) => _sdlPolicyProvider.Add(
			projectId,
			"No High Level Vulnerabilities",
			@"Entity = query SDL policy: no High Level Vulnerabilities;");

		private void CreateRolesForProject(long projectId)
		{
			var developerRoleId = _roleProvider.Create(
				UserRoleAliases.Developer,
				"Developer",
				_queryRepository.Get("Developer tasks query").Select(_ => _.Id).Single(),
				_queryRepository.Get("Developer task results query").Select(_ => _.Id).Single(),
				projectId);

			_authorityProvider.Grant(
				developerRoleId,
				new Authorities.UI().All().Except(
										new[]
										{
											Authorities.UI.Project.ProjectsList.Create,
											Authorities.UI.Administration.View,
											Authorities.UI.Administration.Edit,
											Authorities.UI.Reports.Run,
											Authorities.UI.Queries.EditQueryAll,
											Authorities.UI.Queries.ViewQueriesAll
										}));

			var securityManagerRoleId = _roleProvider.Create(
				UserRoleAliases.Manager,
				"Security manager",
				_queryRepository.Get("Security manager tasks query").Select(_ => _.Id).Single(),
				_queryRepository.Get("Security manager task results query").Select(_ => _.Id).Single(),
				projectId);

			_authorityProvider.Grant(
				securityManagerRoleId,
				new Authorities.UI().All()
									.Except(
										new[]
										{
											Authorities.UI.Administration.View,
											Authorities.UI.Administration.Edit,
											Authorities.UI.Project.ProjectsList.Create,
											Authorities.UI.Project.ProjectsList.Stat.Vulnerabilities,
											Authorities.UI.Project.Settings.ViewAccessControl,
											Authorities.UI.Project.Settings.ViewIssueTracker,
											Authorities.UI.Project.Settings.ViewScanCore,
											Authorities.UI.Project.Settings.ViewVersionControl,
											Authorities.UI.Project.Settings.EditAccessControl,
											Authorities.UI.Project.Settings.EditIssueTracker,
											Authorities.UI.Project.Settings.EditNotifications,
											Authorities.UI.Project.Settings.EditScanCore,
											Authorities.UI.Project.Settings.EditSdlPolicy,
											Authorities.UI.Project.Settings.EditVersionControl,
											Authorities.UI.Queries.EditQueryAll,
											Authorities.UI.Queries.ViewQueriesAll
										}));
		}

		private Plugins GetPlugin(PluginType pluginType)
			=> _pluginProvider.Get(_ => _.Type == (int)pluginType).FirstOrDefault();

		protected override long? GetProjectIdForCommand(CreateProjectCommand command) => null;

		protected override void ProcessAuthorized(CreateProjectCommand command)
		{
			using(var telemetryScope = _telemetryScopeProvider.Create<Projects>(TelemetryOperationNames.Prroject.Create))
			{
				try
				{
					var vcsPlugin = GetPlugin(PluginType.VersionControl);

					var itPlugin = GetPlugin(PluginType.IssueTracker);

					var project = new Projects
					{
						Alias = command.Alias,
						Created = _timeService.GetUtc(),
						CreatedById = _userPrincipal.Info.Id,
						DefaultBranchName = command.DefaultBranchName ?? string.Empty,
						Description = command.Description,
						DisplayName = command.Name,
						ItPluginId = itPlugin?.Id,
						Modified = _timeService.GetUtc(),
						ModifiedById = _userPrincipal.Info.Id,
						SdlPolicyStatus = (int)SdlPolicyStatus.Unknown,
						EnablePoll = false,
						PollTimeout = null,
						VcsPluginId = vcsPlugin?.Id
					};

					telemetryScope.SetEntity(project);

					_repositoryProjects.Insert(project);

					_repositoryProjects.Save();

					CreateRolesForProject(project.Id);

					CreateNotificationRules(project.Id);

					CreatePolicyRules(project.Id);

					command.CreatedProjectId = project.Id;

					telemetryScope.WriteSuccess();
				}
				catch(Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}

		private sealed class NotificationBundle
		{
			public readonly string NotificationReportName;

			public readonly string NotificationRuleName;

			public readonly UserType UserType;

			public NotificationBundle(
				UserType userType,
				[NotNull] string notificationRuleName,
				[NotNull] string notificationReportName)
			{
				if (notificationRuleName == null) throw new ArgumentNullException(nameof(notificationRuleName));
				if (notificationReportName == null) throw new ArgumentNullException(nameof(notificationReportName));

				NotificationRuleName = notificationRuleName;
				NotificationReportName = notificationReportName;

				UserType = userType;
			}
		}

		private enum UserType
		{
			Developer,

			Manager
		}
	}
}