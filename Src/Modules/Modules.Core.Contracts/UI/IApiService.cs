namespace Modules.Core.Contracts.UI
{
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.ServiceModel.Web;

	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Contracts.UI.Dto.Admin;
	using Modules.Core.Contracts.UI.Dto.Data;
	using Modules.Core.Contracts.UI.Dto.ProjectSettings;
	using Modules.Core.Contracts.UI.Dto.UserSettings;

	[ServiceContract]
	public interface IApiService
	{
		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/CheckVersion")]
		void CheckVersion(UserInterfaceInfoDto userInterfaceInfo);

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/User/Current")]
		UserDto GetCurrentUser();

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Users?projectId={projectId}")]
		UserDto[] GetUsers(long projectId);

		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/User/")]
		void UpdateUserInfo(UserUpdatedDto user);

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/User/Roles/?userId={userId}")]
		UserRoleDto[] GetRoles(long userId);

		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/User/Have/Authority/")]
		bool HaveAuthority(AuthorityRequestDto authorityRequest);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/User/Projects/")]
		ProjectDto[] GetProjectsByUser();

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/User/Authorities/")]
		ProjectDto[] GetProjectsByAuthority(IEnumerable<string> authorities);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Admin/Settings/")]
		SettingsDto GetSettings();

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Admin/Settings/")]
		void SetSettings(SettingsDto settings);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Project/")]
		long CreateProject(NewProjectDto project);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/?projectId={projectId}")]
		ProjectDto GetProject(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Branches/?projectId={projectId}")]
		BranchDto[] GetBranches(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Stat/Health?projectId={projectId}")]
		TableDto GetProjectHealthStat(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Stat/Metric?projectId={projectId}")]
		TableDto GetProjectMetricStat(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Stat/Vulnerabilities?projectId={projectId}")]
		TableDto GetProjectVulnerabilitiesStat(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Role/Authorities/?projectId={projectId}&roleId={roleId}")]
		AuthorityDto[] GetProjectAuthoritiesForRole(long projectId, long roleId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Notifications/?projectId={projectId}")]
		NotificationRuleDto[] GetProjectNotificationRules(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Notifications/?id={ruleId}")]
		NotificationRuleDto GetNotificationRule(long ruleId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/Notifications/Save")]
		void SaveNotificationRule(NotificationRuleDto rule);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/Roles/?projectId={projectId}")]
		UserRoleDto[] GetProjectRoles(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/SdlRules/?projectId={projectId}")]
		SdlRuleDto[] GetProjectSdlRules(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/SdlRules/?id={ruleId}")]
		SdlRuleDto GetSdlRule(long ruleId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Project/settings/update/?projectId={projectId}")]
		void UpdateProjectSettings(long projectId, ProjectSettingsDto projectSettings);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/project/plugin/settings?projectId={projectId}&pluginId={pluginId}")]
		void UpdatePluginSettings(long projectId, long pluginId, UpdateProjectPluginSettingsDto settings);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/ScanCore/?projectId={projectId}")]
		ScanCoreDto GetScanCore(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/task?taskId={taskId}")]
		TaskDto GetTask(long taskId);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/project/tasks?projectId={projectId}")]
		TableDto GetTasksByProject(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/plugin/?id={id}")]
		PluginDto GetPlugin(long id);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/plugins/?pluginType={pluginType}")]
		PluginDto[] GetPlugins(PluginTypeDto pluginType);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/project/plugins?projectId={projectId}")]
		PluginDto[] GetPluginsByProject(long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/project/plugin/settings/?projectId={projectId}&pluginId={pluginId}")]
		PluginSettingDto[] GetPluginSettingsForProject(long pluginId, long projectId);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/project/user/plugin/settings/?projectId={projectId}&pluginId={pluginId}")]
		PluginSettingDto[] GetPluginSettingsForUserInProject(long pluginId, long projectId);

		[OperationContract]
		[WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/User/Plugins/Settings/")]
		void UpdateUserPluginSetting(UpdateProjectPluginSettingDto[] settings);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/task/results-table?taskId={taskId}")]
		TableDto GetTaskResultsAsTable(long taskId);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/task/results?taskId={taskId}")]
		TaskResultDto[] GetTaskResults(long taskId);

		[OperationContract]
		[WebInvoke(Method = "GET",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/result/?resultId={resultId}")]
		TaskResultDto GetResult(long resultId);

		[OperationContract]
		[WebInvoke(Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/task/create")]
		long CreateTask(CreateTaskDto createTask);

		[OperationContract]
		[WebInvoke(Method = "POST",
			ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/task/stop?taskId={taskId}")]
		void StopTask(long taskId);

		[OperationContract]
		[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/echo")]
		string Echo();

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/settings/get?owner={owner}&entityId={entityId}")]
		SettingGroupDto[] GetEntitySettings(SettingOwnerDto owner, long entityId);

		[OperationContract]
		[WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/settings/save")]
		KeyValuePair<long, string>[] SaveEntitySettings(SettingValueDto[] values);

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Capabilities?capabilityKey={capabilityKey}")]
		string GetCapability(string capabilityKey);

		[OperationContract]
		[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
			UriTemplate = "/Licence")]
		LicenceInfoDto GetLicenceInfo();
	}
}