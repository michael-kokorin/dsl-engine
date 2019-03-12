namespace Workflow.Event.Handlers
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Common.Enums;
	using Infrastructure;
	using Infrastructure.Events;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class PostprocessingScanTaskFinishedEventHandler: IEventHandler
	{
		private readonly IEventProvider _eventProvider;

		private readonly ITaskRepository _taskRepository;

		public PostprocessingScanTaskFinishedEventHandler(
			[NotNull] ITaskRepository taskRepository,
			[NotNull] IEventProvider eventProvider)
		{
			if(taskRepository == null)
			{
				throw new ArgumentNullException(nameof(taskRepository));
			}

			if(eventProvider == null)
			{
				throw new ArgumentNullException(nameof(eventProvider));
			}

			_taskRepository = taskRepository;
			_eventProvider = eventProvider;
		}

		/// <summary>
		///     Determines whether this instance can handle the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns><see langword="true"/> if this instance can handle the event; otherwise, <see langword="false"/>.</returns>
		public bool CanHandle([NotNull] Event eventToHandle) => eventToHandle.Key == EventKeys.ScanTask.PostprocessingFinished;

		/// <summary>
		///     Handles the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		public void Handle([NotNull] Event eventToHandle)
		{
			var data = eventToHandle.Data;

			if((data == null) || !data.ContainsKey(Variables.TaskId))
			{
				return;
			}

			var taskId = long.Parse(data[Variables.TaskId]);
			var task = _taskRepository.GetById(taskId);

			if(task.TaskResolutionStatus == TaskResolutionStatus.Error)
			{
				_eventProvider.Publish(
					new Event
					{
						Key = EventKeys.ScanTask.Failed,
						Data = new Dictionary<string, string>
								{
									{Variables.ProjectId, task.ProjectId.ToString()},
									{Variables.TaskId, task.Id.ToString()}
								}
					});
			}

			if(task.TaskResolutionStatus == TaskResolutionStatus.Cancelled)
			{
				_eventProvider.Publish(
					new Event
					{
						Key = EventKeys.ScanTask.Cancelled,
						Data = new Dictionary<string, string>
								{
									{Variables.ProjectId, task.ProjectId.ToString()},
									{Variables.TaskId, task.Id.ToString()}
								}
					});
			}

			if(task.Projects.DefaultBranchName != task.Repository)
			{
				return;
			}

			_eventProvider.Publish(
				new Event
				{
					Key = EventKeys.Policy.CheckRequired,
					Data = new Dictionary<string, string>
							{
								{Variables.ProjectId, task.ProjectId.ToString()},
								{Variables.TaskId, task.Id.ToString()}
							}
				});
		}
	}
}