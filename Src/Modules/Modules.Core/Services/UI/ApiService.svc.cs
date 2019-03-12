namespace Modules.Core.Services.UI
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Enums;
	using Common.Logging;
	using Common.Query;
	using Common.Security;
	using Infrastructure;
	using Infrastructure.AD;
	using Infrastructure.Engines.Query.Result;
	using Infrastructure.Mail;
	using Infrastructure.Plugins;
	using Infrastructure.Plugins.Contracts;
	using Infrastructure.Security;
	using Infrastructure.UserInterface;
	using Modules.Core.Contracts.UI;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Contracts.UI.Dto.Admin;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Contracts.UI.Dto.ProjectSettings;
	using Modules.Core.Contracts.UI.Dto.UserSettings;
	using Modules.Core.Helpers;
	using Modules.Core.Services.Query.DataQueries;
	using Modules.Core.Services.UI.Commands;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.Renderers;
	using Repository.Repositories;

	public sealed class ApiService : IApiService
	{
		private readonly IAuthorityProvider _authorityProvider;

		private readonly ICommandDispatcher _commandDispatcher;

		private readonly IDataQueryDispatcher _dataQueryDispatcher;

		private readonly ISettingsHelper _settingsHelper;

		private readonly IConfigurationProvider _configurationProvider;

		private readonly IMailConnectionParametersProvider _mailConnectionParametersProvider;

		private readonly INotificationRuleRepository _notificationRuleRepository;

		private readonly IPluginFactory _pluginFactory;

		private readonly IPolicyRuleRepository _policyRuleRepository;

		private readonly IProjectPluginSettingsProvider _projectPluginSettingsProvider;

		private readonly IProjectRepository _projectRepository;

		private readonly ITaskRepository _taskRepository;

		private readonly ITaskResultRepository _taskResultRepository;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserInfoProvider _userInfoProvider;

		private readonly IUserInterfaceChecker _userInterfaceChecker;

		private readonly IUserPluginSettingsProvider _userPluginSettingsProvider;

		private readonly IUserPrincipal _userPrincipal;

		public ApiService(
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] ICommandDispatcher commandDispatcher,
			[NotNull] ITaskRepository taskRepository,
			[NotNull] IProjectRepository projectRepository,
			[NotNull] INotificationRuleRepository notificationRuleRepository,
			[NotNull] IProjectPluginSettingsProvider projectPluginSettingsProvider,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] IUserPluginSettingsProvider userPluginSettingsProvider,
			[NotNull] IAuthorityProvider authorityProvider,
			[NotNull] IConfigurationProvider configurationProvider,
			[NotNull] IPolicyRuleRepository policyRuleRepository,
			[NotNull] IUserInfoProvider userInfoProvider,
			[NotNull] IMailConnectionParametersProvider mailConnectionParametersProvider,
			[NotNull] ITaskResultRepository taskResultRepository,
			[NotNull] IPluginFactory pluginFactory,
			[NotNull] IUserInterfaceChecker userInterfaceChecker,
			[NotNull] IDataQueryDispatcher dataQueryDispatcher,
			[NotNull] ISettingsHelper settingsHelper)
		{
			_userAuthorityValidator = userAuthorityValidator;
			_commandDispatcher = commandDispatcher;
			_taskRepository = taskRepository;
			_projectRepository = projectRepository;
			_notificationRuleRepository = notificationRuleRepository;
			_projectPluginSettingsProvider = projectPluginSettingsProvider;
			_userPrincipal = userPrincipal;
			_userPluginSettingsProvider = userPluginSettingsProvider;
			_authorityProvider = authorityProvider;
			_configurationProvider = configurationProvider;
			_policyRuleRepository = policyRuleRepository;
			_userInfoProvider = userInfoProvider;
			_mailConnectionParametersProvider = mailConnectionParametersProvider;
			_taskResultRepository = taskResultRepository;
			_pluginFactory = pluginFactory;
			_userInterfaceChecker = userInterfaceChecker;
			_dataQueryDispatcher = dataQueryDispatcher;
			_settingsHelper = settingsHelper;
		}

		[LogMethod(LogInputParameters = true)]
		public void CheckVersion(UserInterfaceInfoDto userInterfaceInfo)
		{
			if (userInterfaceInfo == null)
				throw new ArgumentNullException(nameof(userInterfaceInfo));

			_userInterfaceChecker.Check(userInterfaceInfo.Host, userInterfaceInfo.Version);
		}

		[LogMethod(LogOutputValue = true)]
		public UserDto GetCurrentUser()
		{
			var user = _userPrincipal.Info.ToDto();

			user.CurrentCulture = Thread.CurrentThread.CurrentCulture.Name;

			return user;
		}

		[LogMethod(LogInputParameters = true)]
		public UserDto[] GetUsers(long projectId)
		{
			var query = new GetUsersByProjectQuery(projectId);

			return _dataQueryDispatcher.Process<GetUsersByProjectQuery, UserDto[]>(query);
		}

		[LogMethod(LogOutputValue = true)]
		public void UpdateUserInfo(UserUpdatedDto user)
		{
			if (user == null) throw new ArgumentNullException(nameof(user));

			if (user.Id != _userPrincipal.Info.Id)
				throw new UnauthorizedAccessException();

			_userInfoProvider.Update(user.Id, _userPrincipal.Info.Login, user.DisplayName, user.Email);
		}

		[LogMethod(LogOutputValue = true)]
		public UserRoleDto[] GetRoles(long userId) =>
			_dataQueryDispatcher.Process<GetRolesByUserQuery, UserRoleDto[]>(
				new GetRolesByUserQuery(userId));

		[LogMethod]
		public bool HaveAuthority(AuthorityRequestDto authorityRequest)
		{
			if (authorityRequest == null) throw new ArgumentNullException(nameof(authorityRequest));

			var result = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				authorityRequest.AuthorityNames,
				authorityRequest.EntityId);

			return result;
		}

		[LogMethod]
		public ProjectDto[] GetProjectsByUser()
		{
			var projectIds = _userAuthorityValidator
				.GetProjects(_userPrincipal.Info.Id, new[] {Authorities.UI.Project.ProjectsList.View});

			return _projectRepository.Query()
				.Where(_ => projectIds.Contains(_.Id))
				.Select(new ProjectDtoRenderer().GetSpec())
				.ToArray();
		}

		[LogMethod]
		public ProjectDto[] GetProjectsByAuthority(IEnumerable<string> authorities)
		{
			if (authorities == null)
				throw new ArgumentNullException(nameof(authorities));

			var projectIds = _userAuthorityValidator.GetProjects(_userPrincipal.Info.Id, authorities);

			return _projectRepository.Query()
				.Where(_ => projectIds.Contains(_.Id))
				.Select(new ProjectDtoRenderer().GetSpec())
				.ToArray();
		}

		[LogMethod]
		public SettingsDto GetSettings() =>
			_dataQueryDispatcher.Process<GetSystemSettingsQuery, SettingsDto>(new GetSystemSettingsQuery());

		[LogMethod]
		public void SetSettings(SettingsDto settings)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Administration.Edit
				},
				null))
				throw new UnauthorizedAccessException();

			if (settings.ActiveDirectorySettings != null)
				_configurationProvider.SetValue(
					ConfigurationKeys.AppSettings.ActiveDirectoryRootGroup,
					settings.ActiveDirectorySettings.RootGroupPath);

			if (settings.FileStorageSettings != null)
			{
				_configurationProvider.SetValue(
					ConfigurationKeys.AppSettings.TempDirPath,
					settings.FileStorageSettings.TempDirPath);
			}

			if (settings.NotificationSettings != null)
			{
				_mailConnectionParametersProvider.Set(new MailConnectionParameters
				{
					Host = settings.NotificationSettings.MailServerHost,
					IsSslEnabled = settings.NotificationSettings.IsSslEnabled,
					Mailbox = settings.NotificationSettings.MailBox,
					Password = settings.NotificationSettings.Password,
					Port = settings.NotificationSettings.MainServerPort,
					Username = settings.NotificationSettings.UserName
				});
			}
		}

		[LogMethod]
		public long CreateProject(NewProjectDto project)
		{
			var command = project.ToCommand();

			_commandDispatcher.Handle(command);

			return command.CreatedProjectId;
		}

		[LogMethod(LogInputParameters = true)]
		public ProjectDto GetProject(long projectId)
		{
			var canView = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.ProjectsList.View
				},
				projectId);

			if (!canView)
				throw new UnauthorizedAccessException();

			return _projectRepository.Query()
				.Where(_ => _.Id == projectId)
				.Select(new ProjectDtoRenderer().GetSpec())
				.SingleOrDefault();
		}

		[LogMethod(LogInputParameters = true, LogOutputValue = true)]
		public BranchDto[] GetBranches(long projectId)
		{
			var project = _projectRepository.GetById(projectId);

			if (project == null)
				return new BranchDto[0];

			var projectVcsPluginId = project.VcsPluginId;

			var userId = _userPrincipal.Info.Id;

			if (projectVcsPluginId == null) throw new Exception();

			var vcsPlugin = _pluginFactory.Prepare(projectVcsPluginId.Value, projectId, userId) as IVersionControlPlugin;

			var branches = vcsPlugin?.GetBranches().ToArray();

			if (branches == null)
				return new BranchDto[0];

			var branchDtos = branches.Select(_ => _.ToDto()).ToArray();

			foreach (var branch in branchDtos)
			{
				branch.IsDefault = branch.Id == project.DefaultBranchName;
				branch.LastScanFinishedUtc = _taskRepository.GetLast(projectId, branch.Name)?.Finished;
			}

			return branchDtos;
		}

		[LogMethod(LogInputParameters = true)]
		public TableDto GetProjectHealthStat(long projectId)
		{
			var canView = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.ProjectsList.Stat.Health
				},
				projectId);

			if (!canView)
				return new TableDto();

			var entities = _taskRepository.GetLast(projectId, 3)
				.AsEnumerable()
				.Select(_ => new QueryResultItem
				{
					EntityId = _.Id,
					Value = new
					{
						_.Id,
						Status = _.TaskStatus.ToString(),
						SdlPolicyStatus = ((SdlPolicyStatus) _.SdlStatus).ToString(),
						High = _.HighSeverityVulns,
						Medium = _.MediumSeverityVulns,
						Low = _.LowSeverityVulns
					}
				});

			return new TableRenderer().Render(entities);
		}

		[LogMethod(LogInputParameters = true)]
		public TableDto GetProjectMetricStat(long projectId)
		{
			var canView = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.ProjectsList.Stat.Metrics
				},
				projectId);

			if (!canView)
				return new TableDto();

			var entities = _taskRepository.GetLast(projectId, 3)
				.ToArray()
				.Select(_ => new QueryResultItem
				{
					EntityId = _.Id,
					Value = new
					{
						_.Id,
						Branch = _.Repository,
						LinesCount = _.AnalyzedLinesCount ?? 0,
						AnalyzedSize = _.AnalyzedSize / 1024 ?? 0,
						FolderSize = _.FolderSize / 1024 ?? 0,
						Time = _.ScanCoreWorkingTime / 1000 ?? 0,
						Speed =
							(_.AnalyzedSize != null) &&
							(_.AnalyzedSize > 0) &&
							(_.ScanCoreWorkingTime != null) &&
							(_.ScanCoreWorkingTime > 0)
								? ((double) _.AnalyzedSize / 1024 / ((double) _.ScanCoreWorkingTime / 1000)).ToString(
									"F",
									CultureInfo.CurrentCulture)
								: null
					}
				});

			return new TableRenderer().Render(entities);
		}

		[LogMethod(LogInputParameters = true)]
		public TableDto GetProjectVulnerabilitiesStat(long projectId)
		{
			var canView = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.ProjectsList.Stat.Vulnerabilities
				},
				projectId);

			if (!canView)
				return new TableDto();

			var entities = _taskRepository.GetLast(projectId, 3)
				.SelectMany(_ => _.TaskResults)
				.GroupBy(_ => new
				{
					_.Type,
					_.SeverityType,
					_.TaskId
				})
				.OrderByDescending(_ => _.Key.TaskId)
				.ThenBy(_ => _.Key.SeverityType)
				.Select(_ => new QueryResultItem
				{
					EntityId = _.Key.TaskId,
					Value = new
					{
						Id = _.Key.TaskId,
						Vulnerability = _.Key.Type,
						Severity = ((VulnerabilitySeverityType) _.Key.SeverityType).ToString(),
						Count = _.Count()
					}
				});

			return new TableRenderer().Render(entities);
		}

		[LogMethod(LogInputParameters = true)]
		public AuthorityDto[] GetProjectAuthoritiesForRole(long projectId, long roleId)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Settings.ViewAccessControl
				},
				projectId))
				throw new UnauthorizedAccessException();

			return _authorityProvider.Get(roleId, projectId)
				.Select(new AuthorityRenderer().GetSpec())
				.ToArray();
		}

		[LogMethod(LogInputParameters = true)]
		public NotificationRuleDto[] GetProjectNotificationRules(long projectId)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Settings.ViewNotifications
				},
				projectId))
				throw new UnauthorizedAccessException();

			return _notificationRuleRepository.GetByProject(projectId)
				.Select(new NotificationRuleRenderer().GetSpec())
				.OrderBy(_ => _.DisplayName)
				.ToArray();
		}

		[LogMethod(LogInputParameters = true)]
		public NotificationRuleDto GetNotificationRule(long ruleId)
		{
			var query = new GetNotificationQuery(ruleId);

			return _dataQueryDispatcher.Process<GetNotificationQuery, NotificationRuleDto>(query);
		}

		[LogMethod(LogInputParameters = true)]
		public void SaveNotificationRule(NotificationRuleDto rule) =>
			_commandDispatcher.Handle(new UpdateNotificationRuleCommand
			{
				Query = rule.Query,
				RuleId = rule.Id
			});

		[LogMethod(LogInputParameters = true)]
		public UserRoleDto[] GetProjectRoles(long projectId) =>
			_dataQueryDispatcher.Process<GetRolesByProjectQuery, UserRoleDto[]>(
				new GetRolesByProjectQuery(projectId));

		[LogMethod(LogInputParameters = true)]
		public SdlRuleDto[] GetProjectSdlRules(long projectId)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Settings.ViewSdlPolicy
				},
				projectId))
				throw new UnauthorizedAccessException();

			return _policyRuleRepository.Get(projectId)
				.Select(new SdlRuleRenderer().GetSpec())
				.ToArray();
		}

		[LogMethod(LogInputParameters = true, LogOutputValue = true)]
		public SdlRuleDto GetSdlRule(long ruleId) => _policyRuleRepository.Query()
			.Where(_ => _.Id == ruleId)
			.Select(new SdlRuleRenderer().GetSpec())
			.SingleOrDefault();

		[LogMethod(LogInputParameters = true)]
		public void UpdateProjectSettings(long projectId, ProjectSettingsDto projectSettings) =>
			_commandDispatcher.Handle(new UpdateProjectSettingsCommand
			{
				Alias = projectSettings.Alias,
				CommitToIt = projectSettings.CommitToIt,
				CommitToVcs = projectSettings.CommitToVcs,
				DefaultBranchName = projectSettings.DefaultBranchName,
				Description = projectSettings.Description,
				DisplayName = projectSettings.DisplayName,
				ProjectId = projectId,
				VcsSyncEnabled = projectSettings.VcsSyncEnabled,
				EnablePoll = projectSettings.EnablePoll,
				PollTimeout = projectSettings.PollTimeout
			});

		[LogMethod]
		public void UpdatePluginSettings(long projectId, long pluginId, UpdateProjectPluginSettingsDto settings) =>
			_commandDispatcher.Handle(new UpdateProjectPluginSettingsCommand
			{
				ProjectId = projectId,
				PluginId = pluginId,
				Settings = settings.Settings.Select(_ => _.ToEntity())
			});

		[LogMethod(LogInputParameters = true)]
		public ScanCoreDto GetScanCore(long projectId)
		{
			var cores = new Dictionary<string, string>
			{
				{"sharp-use", "C#"},
				{"java-use", "Java"},
				{"php-use", "PHP"},
				{"fingerprint-use", "Fingerprint"},
				{"pm-use", "Pattern Matching"},
				{"config-use", "Configurations"},
				{"blackbox-use", "Blackbox"}
			};

			var active = cores.Where(
				pair =>
				{
					var setting = _settingsHelper.Get(SettingOwnerDto.Project, projectId, pair.Key);
					if (setting == null)
					{
						return false;
					}

					var value = Convert.ToBoolean(setting.Value);
					return value;
				}).ToList();

			return new ScanCoreDto
			{
				Key = string.Join(", ", active.Select(pair => pair.Key)),
				DisplayName = string.Join(", ", active.Select(pair => pair.Value))
			};
		}

		[LogMethod(LogInputParameters = true)]
		public TaskDto GetTask(long taskId)
		{
			var task = _taskRepository.GetById(taskId);

			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Tasks.ViewResults
				},
				task.ProjectId))
				throw new UnauthorizedAccessException();

			var dto = new TasksRenderer().GetSpec()(task);

			return dto;
		}

		[LogMethod(LogInputParameters = true)]
		public TableDto GetTasksByProject(long projectId)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Tasks.View
				},
				projectId))
				throw new UnauthorizedAccessException();

			var query = new GetProjectTasksQuery(projectId, _userPrincipal.Info.Id);

			var result = _dataQueryDispatcher.Process<GetProjectTasksQuery, TableDto>(query);

			return result;
		}

		[LogMethod(LogInputParameters = true)]
		public PluginDto GetPlugin(long id)
		{
			var query = new GetPluginByIdQuery(id);

			return _dataQueryDispatcher.Process<GetPluginByIdQuery, PluginDto>(query);
		}


		[LogMethod(LogInputParameters = true)]
		public PluginDto[] GetPlugins(PluginTypeDto pluginType)
		{
			var query = new GetPluginsByTypeQuery(pluginType);

			return _dataQueryDispatcher.Process<GetPluginsByTypeQuery, PluginDto[]>(query);
		}

		[LogMethod(LogInputParameters = true)]
		public PluginDto[] GetPluginsByProject(long projectId)
		{
			var query = new GetPluginsByProjectQuery(projectId);

			return _dataQueryDispatcher.Process<GetPluginsByProjectQuery, PluginDto[]>(query);
		}

		[LogMethod(LogInputParameters = true)]
		public PluginSettingDto[] GetPluginSettingsForProject(long pluginId, long projectId)
		{
			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Settings.ViewVersionControl,
					Authorities.UI.Project.Settings.ViewIssueTracker
				},
				projectId))
				throw new UnauthorizedAccessException();

			return _projectPluginSettingsProvider
				.GetSettings(projectId, pluginId)
				.Select(new PluginSettingRenderer().GetSpec())
				.ToArray();
		}

		[LogMethod(LogInputParameters = true)]
		public PluginSettingDto[] GetPluginSettingsForUserInProject(long pluginId, long projectId)
			=> _userPluginSettingsProvider
				.GetSettings(_userPrincipal.Info.Id, projectId, pluginId)
				.Select(new PluginSettingRenderer().GetSpec())
				.ToArray();

		[LogMethod]
		public void UpdateUserPluginSetting(UpdateProjectPluginSettingDto[] settings) =>
			_userPluginSettingsProvider.SetValues(
				_userPrincipal.Info.Id,
				settings.Select(_ => _.ToEntity()));

		[LogMethod(LogInputParameters = true)]
		public TableDto GetTaskResultsAsTable(long taskId)
		{
			var query = new GetTaskResultsQuery(taskId, _userPrincipal.Info.Id);

			return _dataQueryDispatcher.Process<GetTaskResultsQuery, TableDto>(query);

		}

		[LogMethod(LogInputParameters = true)]
		public TaskResultDto[] GetTaskResults(long taskId)
		{
			var result = _taskResultRepository.Query()
				.Where(_ => _.TaskId == taskId)
				.Select(new TaskResultRenderer().GetSpec())
				.ToArray();

			if (result.Any() && !_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Tasks.ViewResults
				},
				result.First().ProjectId))
				throw new UnauthorizedAccessException();

			return result;
		}

		[LogMethod(LogInputParameters = true)]
		public TaskResultDto GetResult(long resultId)
		{
			var result = _taskResultRepository.Query()
				.Where(_ => _.Id == resultId)
				.Select(new TaskResultRenderer().GetSpec())
				.SingleOrDefault();

			if (result == null)
				return null;

			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Tasks.ViewResults
				},
				result.ProjectId))
				throw new UnauthorizedAccessException();

			return result;
		}

		[LogMethod]
		public long CreateTask(CreateTaskDto createTask)
		{
			var command = createTask.ToCommand();
			_commandDispatcher.Handle(command);

			return command.CreatedTaskId;
		}

		[LogMethod(LogInputParameters = true)]
		public void StopTask(long taskId) => _commandDispatcher.Handle(new StopTaskCommand
		{
			TaskId = taskId
		});

		public string Echo() => "Echo success";

		[LogMethod]
		public SettingGroupDto[] GetEntitySettings(SettingOwnerDto owner, long entityId)
			=> _settingsHelper.Get(owner, entityId);

		[LogMethod]
		public KeyValuePair<long, string>[] SaveEntitySettings(SettingValueDto[] values)
			=> _settingsHelper.Save(values).ToArray();

		[LogMethod(LogInputParameters = true)]
		public string GetCapability(string capabilityKey)
		{
			var query = new GetCapabilityByKeyQuery(capabilityKey);

			return _dataQueryDispatcher.Process<GetCapabilityByKeyQuery, string>(query);
		}

		[LogMethod]
		public LicenceInfoDto GetLicenceInfo()
		{
			var query = new GetLicenceInfoQuery();

			return _dataQueryDispatcher.Process<GetLicenceInfoQuery, LicenceInfoDto>(query);
		}
	}
}