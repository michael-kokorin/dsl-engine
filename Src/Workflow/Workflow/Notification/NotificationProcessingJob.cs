using Infrastructure.Scheduler;

namespace Workflow.Notification
{
	using System;

	using JetBrains.Annotations;

	using Quartz;

	[UsedImplicitly]
	[DisallowConcurrentExecution]
	internal sealed class NotificationProcessingJob : ScheduledJob
	{
		private readonly INotificationProcessor _notificationProcessor;

		public NotificationProcessingJob([NotNull] INotificationProcessor notificationProcessor)
		{
			if (notificationProcessor == null) throw new ArgumentNullException(nameof(notificationProcessor));

			_notificationProcessor = notificationProcessor;
		}

		/// <summary>
		///     Executes the job.
		/// </summary>
		/// <returns>
		///     Positive value to repeat the task, negative value or 0 - to finish the task and to wait time interval before the
		///     next run.
		/// </returns>
		protected override int Process() => Convert.ToInt32(_notificationProcessor.ProcessNext());
	}
}