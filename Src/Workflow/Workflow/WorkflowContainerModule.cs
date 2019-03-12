namespace Workflow
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Scheduler;
	using Infrastructure.Scheduler.Extensions;
	using Workflow.Event;
	using Workflow.Event.Handlers;
	using Workflow.Extensions;
	using Workflow.Notification;
	using Workflow.VersionControl;

	public sealed class WorkflowContainerModule: IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) =>
			container
				.RegisterType<IEventHandlerDispatcher, EventHandlerDispatcher>(reuseScope)
				.RegisterType<IEventProcessor, EventProcessor>(reuseScope)
				.RegisterType<INotificationProcessor, NotificationProcessor>(reuseScope)
				.RegisterType<IBranchNameBuilder, BranchNameBuilder>(reuseScope)
				.RegisterType<IVcsSynchronizer, VcsSynchronizer>(reuseScope)
				.RegisterType<ICustomJobInitializer, VcsPollJobInitializer>(typeof(VcsPollJobInitializer).FullName, reuseScope)
				.RegisterType<ICustomJobInitializer, NotificationPollJobInitializer>(
					typeof(NotificationPollJobInitializer).FullName,
					reuseScope)

				// Event handlers
				.RegisterEvent<VcsCommittedEventHandler>(reuseScope)
				.RegisterEvent<ExternalSystemActionEventHandler>(reuseScope)
				.RegisterEvent<NotificationEventHandler>(reuseScope)
				.RegisterEvent<WorkflowEventHandler>(reuseScope)
				.RegisterEvent<PostprocessingScanTaskFinishedEventHandler>(reuseScope)
				.RegisterEvent<PolicyCheckEventHandler>(reuseScope)

				// Scheduled jobs
				.RegisterJob<EventProcessingJob>(reuseScope)
				.RegisterJob<NotificationProcessingJob>(reuseScope)
				.RegisterJob<SyncVcsJob>(reuseScope)
				.RegisterJob<NotificationPollJob>(reuseScope)
				.RegisterJob<VcsPollJob>(reuseScope);
	}
}