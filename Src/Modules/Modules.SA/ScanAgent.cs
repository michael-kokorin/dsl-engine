namespace Modules.SA
{
	using System;
	using System.Collections.Generic;
	using System.Configuration;
	using System.Linq;
	using System.Threading;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Logging;
	using Common.SystemComponents;
	using Infrastructure.Plugins.Agent.Contracts;
	using Infrastructure.RequestHandling.Contracts;
	using Infrastructure.Scheduler;
	using Modules.Core.Contracts.ExternalSystems;
	using Modules.Core.Contracts.SA.Dto;
	using Modules.SA.Config;
	using Modules.SA.Properties;

	using Quartz;

	/// <summary>
	///     Represents scan agent.
	/// </summary>
	[DisallowConcurrentExecution]
	internal sealed class ScanAgent: ScheduledJob
	{
		private readonly ILog _logger;

		private readonly IPluginProvider _pluginProvider;

		private readonly IApiService _apiService;

		private readonly IScanAgentIdProvider _scanAgentIdProvider;

		private readonly ISystemVersionProvider _systemVersionProvider;

		public ScanAgent(
			[NotNull] IApiService service,
			[NotNull] ILog logger,
			[NotNull] IPluginProvider pluginProvider,
			[NotNull] IScanAgentIdProvider scanAgentIdProvider,
			[NotNull] ISystemVersionProvider systemVersionProvider)
		{
			if (service == null) throw new ArgumentNullException(nameof(service));
			if (logger == null) throw new ArgumentNullException(nameof(logger));
			if (pluginProvider == null) throw new ArgumentNullException(nameof(pluginProvider));
			if (scanAgentIdProvider == null) throw new ArgumentNullException(nameof(scanAgentIdProvider));
			if (systemVersionProvider == null) throw new ArgumentNullException(nameof(systemVersionProvider));

			_apiService = service;
			_logger = logger;
			_pluginProvider = pluginProvider;
			_scanAgentIdProvider = scanAgentIdProvider;
			_systemVersionProvider = systemVersionProvider;
		}

		private ScanTaskDto GetNextTask()
		{
			var request = GetRequest(ScanAgentRequests.GetNextTask);

			request.SetData(
				new GetScanTaskDto
				{
					ScanAgentId = _scanAgentIdProvider.Get()
				});

			var response = _apiService.Handle(request);

			if (response.Success)
			{
				return response.GetData<ScanTaskDto>();
			}

			_logger.Fatal(response.Message);

			return null;
		}

		[NotNull]
		private static ApiRequest GetRequest(string requestMethod) =>
			new ApiRequest
			{
				SourceId = null,
				SourceType = SourceTypes.Sa,
				RequestMethod = requestMethod
			};

		private bool IsTaskCancelled(long taskId)
		{
			var request = GetRequest(ScanAgentRequests.IsTaskCancelled);

			request.SetData(
				new IsTaskCancelledRequestDto
				{
					TaskId = taskId
				});

			var response = _apiService.Handle(request);

			if (response.Success)
			{
				return response.GetData<IsTaskCancelledResponseDto>().IsCancelled;
			}

			_logger.Fatal(response.Message);

			return false;
		}

		private void CheckVersion()
		{
			var request = new ApiRequest
			{
				SourceId = null,
				SourceType = SourceTypes.Sa,
				RequestMethod = ScanAgentRequests.CheckVersion
			};

			request.SetData(
				new ScanAgentInfoDto
				{
					AssemblyVersion = _systemVersionProvider.GetSystemVersion(),
					Uid = _scanAgentIdProvider.Get(),
					Version = Settings.Default.Version,
					MachineName = Environment.MachineName
				});

			var response = _apiService.Handle(request);

			if (!response.Success)
			{
				_logger.Fatal(response.Message);

				// TODO: typed exception
				throw new Exception($"Failed to check service version compability. Message='{response.Message}'");
			}

			var checkVersion = response.GetData<ScanAgentSettingsDto>();

			if (!checkVersion.IsCompatible)
			{
				// TODO: typed exception
				throw new Exception(Resources.SAIsNotCompatible.FormatWith(Settings.Default.Version));
			}
		}

		protected override int Process()
		{
			_logger.Trace("Trying to receive next task to scan...");

			CheckVersion();

			var task = GetNextTask();

			if (task == null)
			{
				_logger.Trace("There are no task to scan in queue");

				return 0;
			}

			_logger.Trace($"Task received. TaskId='{task.Id}'");

			var results = new List<ScanTaskResultDto>();

			var localParameters = ConfigurationManager
				.AppSettings
				.AllKeys.ToDictionary(
					appSettingKey => appSettingKey,
					appSettingKey => ConfigurationManager.AppSettings[appSettingKey]);

			foreach (var core in task.Cores)
			{
				var manager = _pluginProvider.GetByKey(core.Core);

				if (manager == null)
				{
					results.Add(
						new ScanTaskResultDto
						{
							Id = task.Id,
							Status = ScanStatus.Failed
						});
					_logger.Error(Resources.CoreManagerIsNotFound.FormatWith(core.Core, task.Id));

					continue;
				}

				CancellationTokenSource source;

				var executionTask = manager.Run(task.Path, core.CodeLocation, core.CoreParameters, localParameters, out source);

				while (!executionTask.Wait(Settings.Default.WaitCheckTaskCancelledTimeout))
				{
					// ReSharper disable once InvertIf
					if (IsTaskCancelled(task.Id))
					{
						source.Cancel();

						_logger.Info($"Task execution cancelled. TaskId='{task.Id}'");
					}
				}

				results.Add(executionTask.Result);
			}

			if (!results.Any())
			{
				SendResults(
					new ScanTaskResultDto
					{
						Id = task.Id,
						Status = ScanStatus.Failed
					});

				return 1;
			}

			var result = new ScanTaskResultDto
			{
				Id = task.Id,
				AnalyzedFiles = string.Join("\n", results.Select(_ => _.AnalyzedFiles)),
				Status = ScanStatus.Success,
				AnalyzedLinesCount = results.Sum(_ => _.AnalyzedLinesCount),
				AnalyzedSizeInBytes = results.Sum(_ => _.AnalyzedSizeInBytes),
				CoreRunTime = new TimeSpan(results.Sum(_ => _.CoreRunTime.Ticks)),
				LogPath = results.First().LogPath,
				ResultPath = results.First().ResultPath
			};

			SendResults(result);

			_logger.Info($"Task successfully processed. TaskId='{task.Id}', ResultPath='{result.ResultPath}'");

			return 1;
		}

		private void SendResults(ScanTaskResultDto results)
		{
			_logger.Debug($"Trying to send task result. TaskId='{results.Id}'");

			var request = GetRequest(ScanAgentRequests.SendResults);

			request.SetData(results);

			var response = _apiService.Handle(request);

			if (!response.Success)
			{
				_logger.Error($"Failed to send task results. Message='{response.Message}'");
			}

			_logger.Debug($"Task results successfully sent. TaskId='{results.Id}'");
		}
	}
}