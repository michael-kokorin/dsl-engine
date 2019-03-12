namespace Workflow.SA
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Time;
	using Infrastructure.Events;
	using Infrastructure.Telemetry;
	using Infrastructure.Vulnerability;
	using Repository.Context;
	using Repository.Repositories;
	using Workflow.GitHub;

	using Quartz;

	/// <summary>
	///     Processes scan task after scanning.
	/// </summary>
	[DisallowConcurrentExecution]
	internal sealed class PostScanTask: ScanTaskJob
	{
		private readonly ITaskRepository _taskRepository;

		private readonly ITaskResultRepository _taskResultRepository;

		private readonly ITimeService _timeService;

		private readonly IVulnerabilitiesProcessor _vulnerabilitiesProcessor;

		private readonly IVulnerabilitySeverityResolver _vulnerabilitySeverityResolver;

		private readonly IVulnerabilityShortTypeResolver _vulnerabilityShortTypeResolver;

		public PostScanTask(
			IEventProvider eventProvider,
			ITaskRepository taskRepository,
			ITaskResultRepository taskResultRepository,
			ITimeService timeService,
			IVulnerabilitiesProcessor vulnerabilitiesProcessor,
			IVulnerabilitySeverityResolver vulnerabilitySeverityResolver,
			IVulnerabilityShortTypeResolver vulnerabilityShortTypeResolver,
			ITelemetryScopeProvider telemetryScopeProvider)
			: base(eventProvider, taskRepository, telemetryScopeProvider)
		{
			_taskRepository = taskRepository;
			_taskResultRepository = taskResultRepository;
			_timeService = timeService;
			_vulnerabilitiesProcessor = vulnerabilitiesProcessor;
			_vulnerabilitySeverityResolver = vulnerabilitySeverityResolver;
			_vulnerabilityShortTypeResolver = vulnerabilityShortTypeResolver;
		}

		/// <summary>
		///     Gets the end name of the event.
		/// </summary>
		/// <value>
		///     The end name of the event.
		/// </value>
		protected override string EndEventName => EventKeys.ScanTask.PostprocessingFinished;

		/// <summary>
		///     Gets the end status.
		/// </summary>
		/// <value>
		///     The end status.
		/// </value>
		protected override TaskStatus EndStatus => TaskStatus.Done;

		/// <summary>
		///     Gets the start name of the event.
		/// </summary>
		/// <value>
		///     The start name of the event.
		/// </value>
		protected override string StartEventName => EventKeys.ScanTask.PostprocessingStarted;

		/// <summary>
		///     Gets the start status.
		/// </summary>
		/// <value>
		///     The start status.
		/// </value>
		protected override TaskStatus StartStatus => TaskStatus.PostProcessing;

		/// <summary>
		///     Gets the take status.
		/// </summary>
		/// <value>
		///     The take status.
		/// </value>
		protected override TaskStatus TakeStatus => TaskStatus.ReadyToPostProcessing;

		private void CompleteTask([NotNull] Tasks task, [NotNull] TaskProcessingResult result)
		{
			var lowResults = 0;
			var mediumResults = 0;
			var highResults = 0;

			var vulnerabilities = result.Todo.Concat(result.Reopen).Select(x => x.VulnerabilityInfo);

			foreach(var vulnerabilityInfo in vulnerabilities)
			{
				var severity = _vulnerabilitySeverityResolver.Resolve(vulnerabilityInfo.Type);

				switch(severity)
				{
					case VulnerabilitySeverityType.Low:
						lowResults++;
						break;

					case VulnerabilitySeverityType.Medium:
						mediumResults++;
						break;

					case VulnerabilitySeverityType.High:
						highResults++;
						break;
					case VulnerabilitySeverityType.Unknown:

						// TODO log severities of such type because they are new for us
						lowResults++;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				var typeShort = _vulnerabilityShortTypeResolver.Resolve(vulnerabilityInfo.Type);
				typeShort = typeShort.Substring(0, Math.Min(10, typeShort.Length));

				_taskResultRepository.Insert(
					new TaskResults
					{
						AdditionalExploitConditions = vulnerabilityInfo.AdditionalExploitConditions,
						Description = vulnerabilityInfo.Description,
						ExploitGraph = vulnerabilityInfo.Exploit,
						File = vulnerabilityInfo.File,
						Function = vulnerabilityInfo.Function,
						LineNumber = vulnerabilityInfo.NumberLine,
						Message = vulnerabilityInfo.Message,
						Place = vulnerabilityInfo.Place,
						RawLine = vulnerabilityInfo.RawLine,
						SeverityTypeInfo = severity == VulnerabilitySeverityType.Unknown
												? VulnerabilitySeverityType.Low
												: severity,
						SourceFile = vulnerabilityInfo.SourceFile,
						TaskId = task.Id,
						Type = vulnerabilityInfo.Type,
						TypeShort = typeShort,
						LinePosition = vulnerabilityInfo.Position,
						IssueNumber = vulnerabilityInfo.IssueNumber,
						IssueUrl = vulnerabilityInfo.IssueUrl
					});
			}

			task.FinishPostProcessing(_timeService.GetUtc());

			task.LowSeverityVulns = lowResults;
			task.MediumSeverityVulns = mediumResults;
			task.HighSeverityVulns = highResults;

			task.FP = result.FalsePositiveAnnotations.Length + result.FalsePositivePairs.Length;
			task.Todo = result.Todo.Length;
			task.Reopen = result.Reopen.Length;
			task.Fixed = result.Fixed.Length;

			var previousTask = _taskRepository.GetPrevious(task);

			if(previousTask == null)
			{
				return;
			}

			task.IncrementFP = task.FP - previousTask.FP;
			task.IncrementFixed = task.Fixed - previousTask.Fixed;
			task.IncrementReopen = task.Reopen - previousTask.Reopen;
			task.IncrementTodo = task.Todo - previousTask.Todo;
		}

		/// <summary>
		///     Processes the task.
		/// </summary>
		/// <param name="task">The task.</param>
		protected override void ProcessTask([NotNull] Tasks task)
		{
			var result = _vulnerabilitiesProcessor.Process(task);
			CompleteTask(task, result);
		}

		/// <summary>
		///     Publishes the end event.
		/// </summary>
		/// <param name="taskId">The task identifier.</param>
		/// <param name="projectId">The project identifier.</param>
		protected override void PublishEndEvent(long taskId, long projectId)
		{
			base.PublishEndEvent(taskId, projectId);

			PublishTaskEvent(EventKeys.ScanTask.Finished, taskId, projectId);
		}
	}
}