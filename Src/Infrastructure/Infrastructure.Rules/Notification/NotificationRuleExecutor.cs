namespace Infrastructure.Rules.Notification
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Engines;
	using Infrastructure.Notifications;
	using Infrastructure.Reports;
	using Infrastructure.Reports.Generation.Stages;
	using Repository;
	using Repository.Context;

	[UsedImplicitly]
	public sealed class NotificationRuleExecutor : IRuleExecutor<INotificationRule, NotificationRuleResult>
	{
		private readonly ILog _logger;

		private readonly INotificationProvider _notificationProvider;

		private readonly IReportBuilder _reportBuilder;

		private readonly IUserDataProvider _userDataProvider;

		private readonly IUserProvider _userProvider;

		public NotificationRuleExecutor(
			[NotNull] ILog logger,
			[NotNull] INotificationProvider notificationProvider,
			[NotNull] IReportBuilder reportBuilder,
			[NotNull] IUserDataProvider userDataProvider,
			[NotNull] IUserProvider userProvider)
		{
			if (logger == null) throw new ArgumentNullException(nameof(logger));
			if (notificationProvider == null) throw new ArgumentNullException(nameof(notificationProvider));
			if (reportBuilder == null) throw new ArgumentNullException(nameof(reportBuilder));
			if (userDataProvider == null) throw new ArgumentNullException(nameof(userDataProvider));
			if (userProvider == null) throw new ArgumentNullException(nameof(userProvider));

			_logger = logger;
			_notificationProvider = notificationProvider;
			_reportBuilder = reportBuilder;
			_userDataProvider = userDataProvider;
			_userProvider = userProvider;
		}

		public NotificationRuleResult Execute(INotificationRule rule, IDictionary<string, string> parameters)
		{
			var bundle = new NotificationRuleBundle(rule, parameters);

			var targetUsers = GetTargetUsers(rule, parameters);

			foreach (var targetUser in targetUsers)
			{
				CreateAndSendNotification(bundle, targetUser);
			}

			return new NotificationRuleResult();
		}

		private void CreateAndSendNotification(NotificationRuleBundle bundle, Users targetUser)
		{
			var userAddress = _userDataProvider.GetDeliveryContacts(bundle.NotificationRule.DeliveryProtocol, targetUser);

			if (string.IsNullOrEmpty(userAddress))
			{
				_logger.Warning(Resources.Resources.UserHasntContacts.FormatWith(
					targetUser.Login,
					bundle.NotificationRule.DeliveryProtocol));

				return;
			}

			var notificationParameters = new ConcurrentDictionary<string, object>();

			foreach (var attachmentParameter in bundle.NotificationNotificationParameters)
			{
				notificationParameters.AddOrUpdate(attachmentParameter.Key,
					attachmentParameter.Value,
					(s, o) => attachmentParameter.Value);
			}

			notificationParameters.AddOrUpdate(DefaultReportParameters.ContainerUseTable, true, (s, o) => true);

			var notificationReport = _reportBuilder.Build(
				bundle.NotificationRule.Expression.ReportId,
				targetUser.Id,
				notificationParameters,
				ReportFileType.Html);

			var notification = new Notification
			{
				Attachments = GetAttachments(bundle, targetUser),
				Message = notificationReport.RawHtml,
				Protocol = bundle.NotificationRule.DeliveryProtocol,
				Targets = new[] {userAddress},
				Title = notificationReport.Title
			};

			_notificationProvider.Publish(notification);
		}

		private NotificationAttachment[] GetAttachments(NotificationRuleBundle bundle, IEntity targetUser)
		{
			if (bundle.NotificationRule.Expression.Attachments == null) return null;

			var notificationsList = new List<NotificationAttachment>();

			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (var attachment in bundle.NotificationRule.Expression.Attachments)
			{
				var reportType = (ReportFileType) Enum.Parse(typeof(ReportFileType), attachment.ExportFormat);

				var attachmentParameters = new ConcurrentDictionary<string, object>();

				foreach (var attachmentParameter in attachment.Parameters)
				{
					attachmentParameters.AddOrUpdate(attachmentParameter.Key,
						attachmentParameter.Value,
						(s, o) => attachmentParameter.Value);
				}

				foreach (var notificationParameter in bundle.NotificationNotificationParameters)
				{
					attachmentParameters.AddOrUpdate(notificationParameter.Key,
						notificationParameter.Value,
						(s, o) => notificationParameter.Value);
				}

				var notificationAttachment = _reportBuilder.Build(
					attachment.ReportId,
					targetUser.Id,
					attachmentParameters,
					reportType);

				notificationsList.Add(new NotificationAttachment
				{
					Content = notificationAttachment.Content,
					Title = notificationAttachment.FileName
				});
			}

			return notificationsList.ToArray();
		}

		private IEnumerable<Users> GetTargetUsers(INotificationRule rule, IDictionary<string, string> parameters)
		{
			var projectId = parameters.ContainsKey(Variables.ProjectId)
				? long.Parse(parameters[Variables.ProjectId])
				: default(long?);

			var targetUsers = _userProvider.GetUsers(rule.Expression.Subjects, projectId);

			return targetUsers;
		}
	}
}