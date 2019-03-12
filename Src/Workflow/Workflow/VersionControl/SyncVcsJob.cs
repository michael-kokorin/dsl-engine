namespace Workflow.VersionControl
{
	using System;

	using JetBrains.Annotations;

	using Infrastructure.Scheduler;

	using Quartz;

	[UsedImplicitly]
	[DisallowConcurrentExecution]
	internal sealed class SyncVcsJob: ScheduledJob
	{
		private readonly IVcsSynchronizer _vcsSynchronizer;

		public SyncVcsJob([NotNull] IVcsSynchronizer vcsSynchronizer)
		{
			if(vcsSynchronizer == null)
			{
				throw new ArgumentNullException(nameof(vcsSynchronizer));
			}

			_vcsSynchronizer = vcsSynchronizer;
		}

		/// <summary>
		///     Executes the job.
		/// </summary>
		/// <returns>
		///     Positive value to repeat the task, negative value or 0 - to finish the task and to wait time interval before the
		///     next run.
		/// </returns>
		protected override int Process() => _vcsSynchronizer.Synchronize();
	}
}