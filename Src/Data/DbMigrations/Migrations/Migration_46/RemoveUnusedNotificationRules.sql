DELETE FROM [data].[NotificationRules]
WHERE [DisplayName] NOT IN (
	'DeveloperPolicyViolation',
	'ManagerPolicyViolation',
	'DeveloperPolicySuccessful',
	'ManagerPolicySuccessful',
	'DeveloperTaskFinished',
	'ManagerTaskFinished'
)