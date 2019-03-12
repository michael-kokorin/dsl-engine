namespace Infrastructure.RequestHandling
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Logging;
	using Infrastructure.Events;
	using Infrastructure.Plugins.Agent.Contracts;
	using Infrastructure.Plugins.Common.Contracts;
	using Infrastructure.RequestHandling.Contracts;
	using Modules.Core.Contracts.SA.Dto;
	using Repository.Context;
	using Repository.Repositories;

	/// <summary>
	///     Provides handling requests for SA.
	/// </summary>
	/// <seealso cref="Infrastructure.RequestHandling.RequestExecutor"/>
	[UsedImplicitly]
	internal sealed class ScanAgentRequestExecutor: RequestExecutor
	{
		private readonly IEventProvider _eventProvider;

		private readonly ISAParameterTranslatorProvider _parameterTranslatorProvider;

		private readonly IScanAgentRepository _scanAgentRepository;

		private readonly ISettingValuesRepository _settingValuesRepository;

		private readonly ITaskRepository _taskRepository;

		public ScanAgentRequestExecutor(
			[NotNull] IScanAgentRepository scanAgentRepository,
			[NotNull] ILog logger,
			[NotNull] IEventProvider eventProvider,
			[NotNull] ITaskRepository taskRepository,
			[NotNull] ISettingValuesRepository settingValuesRepository,
			[NotNull] ISAParameterTranslatorProvider parameterTranslatorProvider)
			: base(logger)
		{
			if(scanAgentRepository == null)
			{
				throw new ArgumentNullException(nameof(scanAgentRepository));
			}
			if(eventProvider == null)
			{
				throw new ArgumentNullException(nameof(eventProvider));
			}
			if(taskRepository == null)
			{
				throw new ArgumentNullException(nameof(taskRepository));
			}
			if(settingValuesRepository == null)
			{
				throw new ArgumentNullException(nameof(settingValuesRepository));
			}
			if(parameterTranslatorProvider == null)
			{
				throw new ArgumentNullException(nameof(parameterTranslatorProvider));
			}

			_scanAgentRepository = scanAgentRepository;
			_eventProvider = eventProvider;
			_taskRepository = taskRepository;
			_settingValuesRepository = settingValuesRepository;
			_parameterTranslatorProvider = parameterTranslatorProvider;
		}

		/// <summary>
		///     Determines whether this instance the specified request can be handled asynchronous.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns><see langword="true"/> if the request can be handled async; otherwise, <see langword="false"/>.</returns>
		public override bool CanHandleAsync(ApiRequest request) => request.RequestMethod == ScanAgentRequests.SendResults;

		private ScanAgentSettingsDto CheckVersion(ScanAgentInfoDto info)
		{
			if (string.IsNullOrEmpty(info.Uid))
				throw new ArgumentException("Scan agent Uid is empty."); // TODO: typed exception

			var scanAgent = _scanAgentRepository.GetByUid(info.Uid);

			// ReSharper disable once InvertIf
			if (scanAgent == null)
			{
				scanAgent = new ScanAgents
				{
					AssemblyVersion = info.AssemblyVersion,
					Machine = info.MachineName,
					Uid = info.Uid,
					Version = info.Version
				};

				_scanAgentRepository.Insert(scanAgent);
				_scanAgentRepository.Save();
			}
			else if (
				(scanAgent.AssemblyVersion != info.AssemblyVersion) ||
				(scanAgent.Machine != info.MachineName) ||
				(scanAgent.Version != info.Version))
			{
				scanAgent.AssemblyVersion = info.AssemblyVersion;
				scanAgent.Machine = info.MachineName;
				scanAgent.Version = info.Version;

				_scanAgentRepository.Save();
			}

			return new ScanAgentSettingsDto
			{
				IsCompatible = true, // TODO: Agent compability validator validator
				ScanAgentId = scanAgent.Id.ToString()
			};
		}

		/// <summary>
		///     Gets the next task.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <returns></returns>
		private ScanTaskDto GetNextTask([NotNull] GetScanTaskDto info)
		{
			if(info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			var task = _taskRepository
				.GetByStatus(TaskStatus.ReadyToScan)
				.OrderBy(x => x.Created)
				.FirstOrDefault();

			if(task == null)
			{
				return null;
			}

			long scanAgentId;

			try
			{
				scanAgentId = _scanAgentRepository.GetByUid(info.ScanAgentId).Id;
			}
			catch(Exception exc)
			{
				throw new Exception($"Incorrect scan agent identifier. ScanAgentId='{info.ScanAgentId}'", exc);
			}

			task.StartScanning(scanAgentId);

			_eventProvider.Publish(
				new Event
				{
					Key = EventKeys.ScanTask.ScanningStarted,
					Data = new Dictionary<string, string>
					{
						{
							Variables.ProjectId, task.ProjectId.ToString()
						},
						{
							Variables.TaskId, task.Id.ToString()
						},
						{
							Variables.ScanAgentId, info.ScanAgentId
						}
					}
				});

			_taskRepository.Save();

			var settings =
				_settingValuesRepository
					.Query()
					.Where(
						_ => (_.EntityId == task.ProjectId) &&
							(_.Settings.SettingOwner == (int)SettingOwner.Project) &&
							!_.Settings.IsArchived)
					.ToArray();

			var coreKeys = new Dictionary<string, string>
			{
				{
					"sharp", "sharp"
				},
				{
					"java", "java"
				},
				{
					"php", "php"
				},
				{
					"pm", "pattern_matching"
				},
				{
					"fingerprint", "fingerprint"
				},
				{
					"config", "configurations"
				},
				{
					"blackbox", "blackbox"
				}
			};

			var activeCores = coreKeys.Where(
				coreKey =>
				{
					var setting = settings.FirstOrDefault(_ => _.Settings.Code == coreKey.Key + "-use");
					if(setting == null)
					{
						return false;
					}

					var value = Convert.ToBoolean(setting.Value);
					return value;
				}).ToArray();

			var taskParameters = new Dictionary<string, string>
			{
				{"FolderPath", task.FolderPath}
			};

			var cores = activeCores.Select(
				_ =>
				{
					var coreSettings = settings
						.Where(coreSetting => coreSetting.Settings.Code.StartsWith(_.Key + "-", StringComparison.Ordinal)).ToArray();

					var parameters = coreSettings.Select(
							coreSetting =>
								_parameterTranslatorProvider.Get(
										coreSetting.Settings.Code.Replace(_.Key + "-", string.Empty),
										taskParameters)?
									.Translate(coreSetting.Value))
						.Where(value => !string.IsNullOrWhiteSpace(value))
						.ToSeparatedString(" ");

					var path = task.FolderPath;
					var codeLocation =
						coreSettings.FirstOrDefault(cs => cs.Settings.Code.Replace(_.Key + "-", string.Empty) == "code-location");
					if(!string.IsNullOrWhiteSpace(codeLocation?.Value))
					{
						path = Path.Combine(path, codeLocation.Value);
					}

					return new ScanTaskCoreDto
					{
						Core = _.Value,
						CodeLocation = path,
						CoreParameters = parameters
					};
				}).ToArray();

			return new ScanTaskDto
			{
				Id = task.Id,
				Path = task.FolderPath,
				Cores = cores
			};
		}

		/// <summary>
		///     Handles the request.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <returns>The request execution result.</returns>
		public override object HandleRequest(ApiRequest request)
		{
			switch(request.RequestMethod)
			{
				case ScanAgentRequests.CheckVersion:
					return CheckVersion(request.GetData<ScanAgentInfoDto>());
				case ScanAgentRequests.GetNextTask:
					return GetNextTask(request.GetData<GetScanTaskDto>());
				case ScanAgentRequests.IsTaskCancelled:
					return IsTaskCancelled(request.GetData<IsTaskCancelledRequestDto>());
				case ScanAgentRequests.SendResults:
					return SendResults(request.GetData<ScanTaskResultDto>());
				default:
					throw new InvalidOperationException();
			}
		}

		private IsTaskCancelledResponseDto IsTaskCancelled(IsTaskCancelledRequestDto request)
		{
			var task = _taskRepository.GetById(request.TaskId);
			if(task == null)
			{
				return null;
			}

			return new IsTaskCancelledResponseDto
			{
				TaskId = request.TaskId,
				IsCancelled = task.IsCancelled
			};
		}

		private bool SendResults(ScanTaskResultDto result)
		{
			var task = _taskRepository.GetById(result.Id);

			string eventKey;
			if(result.Status == ScanStatus.Stopped)
			{
				task.Cancel();
				eventKey = EventKeys.ScanTask.Finished;
			}
			else if(result.Status != ScanStatus.Success)
			{
				task.Fail();
				eventKey = EventKeys.ScanTask.Finished;
			}
			else
			{
				task.FinishScanning();
				eventKey = EventKeys.ScanTask.ScanningFinished;
			}

			task.LogPath = result.LogPath;
			task.ResultPath = result.ResultPath;
			task.ScanCoreWorkingTime = (long)result.CoreRunTime.TotalMilliseconds;
			task.AnalyzedFiles = result.AnalyzedFiles;
			task.AnalyzedLinesCount = result.AnalyzedLinesCount;
			task.AnalyzedSize = result.AnalyzedSizeInBytes;

			_taskRepository.Save();

			_eventProvider.Publish(
				new Event
				{
					Key = eventKey,
					Data = new Dictionary<string, string>
					{
						{
							Variables.TaskId, task.Id.ToString()
						},
						{
							Variables.ProjectId, task.ProjectId.ToString()
						}
					}
				});

			return true;
		}
	}
}