namespace Workflow.SA
{
	using System;
	using System.IO;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.FileSystem;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Plugins.Contracts;
	using Infrastructure.Telemetry;
	using Infrastructure.Telemetry.Entities;
	using Repository.Context;
	using Repository.Repositories;
	using Workflow.GitHub;
	using Workflow.SA.Properties;

	using Quartz;

	/// <summary>
	///     Processes scan task before scanning process.
	/// </summary>
	[DisallowConcurrentExecution]
	internal sealed class PreScanTaskJob: ScanTaskJob
	{
		private readonly IBackendPluginProvider _backendPluginProvider;

		private readonly IConfigurationProvider _configurationProvider;

		private readonly IFileSystemInfoProvider _fileSystemInfoProvider;

		private readonly IProjectRepository _projectRepository;

		private readonly ITelemetryScopeProvider _telemetryScopeProvider;

		public PreScanTaskJob(
			IEventProvider eventProvider,
			ITaskRepository taskRepository,
			IBackendPluginProvider backendPluginProvider,
			IConfigurationProvider configurationProvider,
			IFileSystemInfoProvider fileSystemInfoProvider,
			IProjectRepository projectRepository,
			ITelemetryScopeProvider telemetryScopeProvider,
			ITelemetryScopeProvider telemetryScopeProvider1)
			: base(eventProvider, taskRepository, telemetryScopeProvider)
		{
			_backendPluginProvider = backendPluginProvider;
			_configurationProvider = configurationProvider;
			_fileSystemInfoProvider = fileSystemInfoProvider;
			_projectRepository = projectRepository;
			_telemetryScopeProvider = telemetryScopeProvider1;
		}

		/// <summary>
		///     Gets the end name of the event.
		/// </summary>
		/// <value>
		///     The end name of the event.
		/// </value>
		protected override string EndEventName => EventKeys.ScanTask.PreprocessingFinished;

		/// <summary>
		///     Gets the end status.
		/// </summary>
		/// <value>
		///     The end status.
		/// </value>
		protected override TaskStatus EndStatus => TaskStatus.ReadyToScan;

		/// <summary>
		///     Gets the start name of the event.
		/// </summary>
		/// <value>
		///     The start name of the event.
		/// </value>
		protected override string StartEventName => EventKeys.ScanTask.PreprocessingStarted;

		/// <summary>
		///     Gets the start status.
		/// </summary>
		/// <value>
		///     The start status.
		/// </value>
		protected override TaskStatus StartStatus => TaskStatus.PreProcessing;

		/// <summary>
		///     Gets the take status.
		/// </summary>
		/// <value>
		///     The take status.
		/// </value>
		protected override TaskStatus TakeStatus => TaskStatus.New;

		[NotNull]
		private static string GetSafeRepoName([NotNull] string path) => path
			.Replace("\\", "_")
			.Replace("/", "_")
			.Replace("$", "_");

		/// <summary>
		///     Processes the task.
		/// </summary>
		/// <param name="task">The task.</param>
		protected override void ProcessTask([NotNull] Tasks task)
		{
			var project = _projectRepository.GetById(task.ProjectId);

			if(project.VcsPluginId == null)
			{
				throw new Exception("Failed to preprocess task. Vcs plugin is eq NULL.");
			}

			var plugin = _backendPluginProvider.GetPlugin<IVersionControlPlugin>(
				project.VcsPluginId.Value,
				task.CreatedById,
				task.ProjectId);

			if(plugin == null)
			{
				throw new Exception(Resources.VcsPluginNotFound);
			}

			var folderPath = _configurationProvider.GetValue(ConfigurationKeys.AppSettings.TempDirPath);

			var repoFolder = Path.Combine(task.Repository, task.Id.ToString());

			var path = Path.Combine(folderPath, GetSafeRepoName(repoFolder));

			if(Directory.Exists(path))
			{
				Directory.Delete(path, true);
			}

			Directory.CreateDirectory(path);

			using(var telemetryScope = _telemetryScopeProvider.Create<VcsPluginInfo>(
				TelemetryOperationNames.VcsPlugin.Checkout))
			{
				try
				{
					var vcsPlugin = new VcsPluginInfo
					{
						Plugin = project.Plugins1,
						TaskId = task.Id
					};

					telemetryScope.SetEntity(vcsPlugin);

					plugin.GetSources(task.Repository, path);

					var folderSize = _fileSystemInfoProvider.CalculateDirectorySize(path);

					vcsPlugin.DownloadedSourcesSize = folderSize;

					task.FolderPath = path;
					task.FolderSize = folderSize;

					telemetryScope.WriteSuccess();
				}
				catch(Exception ex)
				{
					telemetryScope.WriteException(ex);

					throw;
				}
			}
		}
	}
}