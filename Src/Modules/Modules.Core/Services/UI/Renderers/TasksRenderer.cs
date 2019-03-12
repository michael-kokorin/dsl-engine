namespace Modules.Core.Services.UI.Renderers
{
	using System;

	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	internal sealed class TasksRenderer: IDataRenderer<Tasks, TaskDto>
	{
		public Func<Tasks, TaskDto> GetSpec() =>
			_ =>
			new TaskDto
			{
				AnalyzedSizeKb = _.AnalyzedSize / 1024, // bytes to KyloBytes
				AnalyzedLinesCount = _.AnalyzedLinesCount,
				Created = _.Created,
				CreatedBy = _.Users.DisplayName,
				Finished = _.Finished,
				FolderPath = _.FolderPath,
				FolderSizeKb = _.FolderSize / 1024, // bytes to KyloBytes
				Id = _.Id,
				ItPluginId = _.Projects.ItPluginId,
				Modified = _.Modified,
				ModifiedBy = _.Users1.DisplayName,
				ProjectId = _.ProjectId,
				Repository = _.Repository,
				ScanCoreWorkTimeSec = _.ScanCoreWorkingTime / 1000, // ms to s
				SdlPolicyStatus = (SdlPolicyStatusDto)_.SdlStatus,
				SeverityLowCount = _.LowSeverityVulns,
				SeverityMediumCount = _.MediumSeverityVulns,
				SeverityHighCount = _.HighSeverityVulns,
				Status = (TaskStatusDto)_.Status,
				VcsPluginId = _.Projects.VcsPluginId,
				ResolutionStatus = (TaskResolutionStatusDto)_.Resolution,
				ResolutionMessage = _.ResolutionMessage
			};
	}
}