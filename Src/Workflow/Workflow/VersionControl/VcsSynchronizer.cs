namespace Workflow.VersionControl
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Time;
	using Infrastructure;
	using Infrastructure.Events;
	using Infrastructure.Plugins;
	using Infrastructure.Plugins.Contracts;
	using Repository.Context;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class VcsSynchronizer: IVcsSynchronizer
	{
		private readonly IBranchNameBuilder _branchNameBuilder;

		private readonly IEventProvider _eventProvider;

		private readonly IProjectRepository _projectRepository;

		private readonly IPluginFactory _pluginFactory;

		private readonly ITimeService _timeService;

		private readonly Regex _systemCommitLabelTemplate = new Regex(@"\[[^\]]+\] in \[[^\]]+\]");

		public VcsSynchronizer(
			[NotNull] IBranchNameBuilder branchNameBuilder,
			[NotNull] IEventProvider eventProvider,
			[NotNull] IProjectRepository projectRepository,
			[NotNull] IPluginFactory pluginFactory,
			[NotNull] ITimeService timeService)
		{
			if(branchNameBuilder == null)
			{
				throw new ArgumentNullException(nameof(branchNameBuilder));
			}

			if(eventProvider == null)
			{
				throw new ArgumentNullException(nameof(eventProvider));
			}

			if(projectRepository == null)
			{
				throw new ArgumentNullException(nameof(projectRepository));
			}

			if(pluginFactory == null)
			{
				throw new ArgumentNullException(nameof(pluginFactory));
			}

			if(timeService == null)
			{
				throw new ArgumentNullException(nameof(timeService));
			}

			_projectRepository = projectRepository;
			_pluginFactory = pluginFactory;
			_branchNameBuilder = branchNameBuilder;
			_eventProvider = eventProvider;
			_timeService = timeService;
		}

		public int Synchronize()
		{
			var syncDateTimc = _timeService.GetUtc();

			var projects = GetProjects();

			if((projects == null) ||
				!projects.Any())
			{
				return 0;
			}

			var processed = 0;

			foreach(var project in projects)
			{
				var vcsPlugin = GetPlugin(project);

				if(vcsPlugin == null)
				{
					continue;
				}

				var lastSyncDate = project.VcsLastSyncUtc?.AsUtc();

				var checkins = vcsPlugin.GetCommits(lastSyncDate, syncDateTimc).ToArray();

				processed += checkins.Length;

				if(checkins.Length == 0)
				{
					continue;
				}

				var branches = checkins
					.Where(_ => string.IsNullOrWhiteSpace(_.Title) || !_systemCommitLabelTemplate.IsMatch(_.Title))
					.GroupBy(_ => _.Branch)
					.Select(_ => _.Key)
					.ToArray();

				// ReSharper disable once LoopCanBePartlyConvertedToQuery
				foreach(var branch in branches)
				{
					if(_branchNameBuilder.IsOurBranch(branch) || branch.Equals(project.DefaultBranchName))
					{
						SendEvent(project.Id, branch);
					}
				}

				project.SetVcsLastScan(syncDateTimc);
			}

			return processed;
		}

		[CanBeNull]
		private IVersionControlPlugin GetPlugin([CanBeNull] Projects project)
		{
			if(project?.VcsPluginId == null)
			{
				return null;
			}

			var plugin = _pluginFactory.Prepare(project.VcsPluginId.Value, project.Id);

			var vcsPlugin = plugin as IVersionControlPlugin;
			return vcsPlugin;
		}

		private IQueryable<Projects> GetProjects()
		{
			var projects = _projectRepository.VcsSyncEnabled();

			return projects;
		}

		private void SendEvent(long projectId, string branchName) =>
			_eventProvider.Publish(
				new Event
				{
					Key = EventKeys.VcsCommitted,
					Data = new Dictionary<string, string>
							{
								{Variables.ProjectId, projectId.ToString()},
								{Variables.Branch, branchName}
							}
				});
	}
}