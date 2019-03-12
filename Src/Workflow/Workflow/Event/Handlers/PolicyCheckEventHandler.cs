namespace Workflow.Event.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Rules;
	using Infrastructure.Rules.Policy;
	using Infrastructure.Telemetry;
	using Repository.Context;
	using Repository.Repositories;
	using Workflow.Properties;

	[UsedImplicitly]
	internal sealed class PolicyCheckEventHandler: IEventHandler
	{
		private readonly IEventProvider _eventProvider;

		private readonly ILog _logger;

		private readonly IPolicyRuleRepository _policyRuleRepository;

		private readonly IRuleExecutorDirector _ruleExecutorDirector;

		private readonly IRuleParser _ruleParser;

		private readonly ITaskRepository _taskRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		public PolicyCheckEventHandler(
			[NotNull] ITaskRepository taskRepository,
			[NotNull] IPolicyRuleRepository policyRuleRepository,
			[NotNull] IRuleExecutorDirector ruleExecutorDirector,
			[NotNull] IEventProvider eventProvider,
			[NotNull] IRuleParser ruleParser,
			[NotNull] ILog logger,
			[NotNull] ITelemetryScopeProvider telemetryScopeProvider)
		{
			if(taskRepository == null) throw new ArgumentNullException(nameof(taskRepository));
			if(policyRuleRepository == null) throw new ArgumentNullException(nameof(policyRuleRepository));
			if(ruleExecutorDirector == null) throw new ArgumentNullException(nameof(ruleExecutorDirector));
			if(eventProvider == null) throw new ArgumentNullException(nameof(eventProvider));
			if(ruleParser == null) throw new ArgumentNullException(nameof(ruleParser));
			if(logger == null) throw new ArgumentNullException(nameof(logger));
			if(telemetryScopeProvider == null) throw new ArgumentNullException(nameof(telemetryScopeProvider));

			_taskRepository = taskRepository;
			_policyRuleRepository = policyRuleRepository;
			_ruleExecutorDirector = ruleExecutorDirector;
			_eventProvider = eventProvider;
			_ruleParser = ruleParser;
			_logger = logger;
			_telemetryScopeProvider = telemetryScopeProvider;
		}

		/// <summary>
		///   Determines whether this instance can handle the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		/// <returns><see langword="true"/> if this instance can handle the event; otherwise, <see langword="false"/>.</returns>
		public bool CanHandle(Event eventToHandle) => eventToHandle.Key == EventKeys.Policy.CheckRequired;

		/// <summary>
		///   Handles the specified event to handle.
		/// </summary>
		/// <param name="eventToHandle">The event to handle.</param>
		public void Handle(Event eventToHandle)
		{
			PublishPolicyEvent(eventToHandle, EventKeys.Policy.CheckStarted);

			Tasks task = null;

			using(var telemetryScope = _telemetryScopeProvider.Create<Tasks>(TelemetryOperationNames.Task.Update))
			{

				try
				{
					var data = eventToHandle.Data;

					if((data == null) || !data.ContainsKey(Variables.TaskId))
					{
						PublishPolicyEvent(eventToHandle, EventKeys.Policy.CheckFinished);

						return;
					}

					var taskId = long.Parse(data[Variables.TaskId]);

					task = _taskRepository.GetById(taskId);

					telemetryScope.SetEntity(task);

					var projectId = long.Parse(data[Variables.ProjectId]);
					var rules = _policyRuleRepository.Get(projectId).ToArray();

					var policyRules = rules.Select(_ => _ruleParser.ParsePolicyRule(_)).ToArray();

					var success = true;

					var violatedRules = new List<IPolicyRule>();

					// ReSharper disable once LoopCanBePartlyConvertedToQuery
					foreach(var policyRule in policyRules)
					{
						var ruleResult = _ruleExecutorDirector.Execute<IPolicyRule, PolicyRuleResult>(policyRule, data);

						if(ruleResult.Success) continue;

						success = false;

						violatedRules.Add(policyRule);
					}

					if(!success)
					{
						task.ViolatePolicy(
							Resources.PolicyViolated
									.FormatWith(violatedRules.Select(_ => _.Rule.Name).ToCommaSeparatedString()));
					}
					else
						task.SuccessPolicy();

					_taskRepository.Save();

					PublishPolicyEvent(eventToHandle, EventKeys.Policy.CheckFinished);
					PublishPolicyEvent(eventToHandle, success ? EventKeys.Policy.Successful : EventKeys.Policy.Violation);

					telemetryScope.WriteSuccess();
				}
				catch(Exception exception)
				{
					_logger.Error(Resources.PolicyCheckingError, exception);

					try
					{
						task?.ErrorPolicy(Resources.PolicyError.FormatWith(exception.Message, exception.Format()));
						_taskRepository.Save();
					}
					catch(Exception innerException)
					{
						_logger.Error(Resources.PolicyCheckingError, innerException);
					}

					PublishPolicyEvent(eventToHandle, EventKeys.Policy.CheckFinished);
					PublishPolicyEvent(eventToHandle, EventKeys.Policy.Error);

					telemetryScope.WriteException(exception);
				}
			}
		}

		private void PublishPolicyEvent(Event eventToHandle, string key) =>
			_eventProvider.Publish(
				new Event
				{
					Key = key,
					Data = eventToHandle.Data
				});
	}
}