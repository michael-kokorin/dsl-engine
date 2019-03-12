namespace Modules.Core.Environment
{
	using Common.FileSystem;

	internal sealed class EnvironmentConstants
	{
		public const string DeveloperFtpTechByTrigger = "DeveloperFtpTechByTrigger";

		public const string DeveloperPolicyViolationRuleName = "DeveloperPolicyViolation";

		public const string ManagerPolicyViolationRuleName = "ManagerPolicyViolation";

		public const string DeveloperPolicySuccessfulRuleName = "DeveloperPolicySuccessful";

		public const string ManagerFtpAnalystReportByTrigget = "ManagerFtpAnalystReportByTrigget";

		public const string ManagerPolicySuccessfulRuleName = "ManagerPolicySuccessful";

		public const string DeveloperTaskFinishedRuleName = "DeveloperTaskFinished";

		public const string ManagerTaskFinishedRuleName = "ManagerTaskFinished";

		public string GetNotificationRule(string ruleName) =>
			FileLoader.FromResource($"{GetType().Namespace}.NotificationRules.{ruleName}.txt");
	}
}