namespace Modules.Core.Services.UI.Renderers
{
	using System;

	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	internal sealed class ProjectDtoRenderer : IDataRenderer<Projects, ProjectDto>
	{
		public Func<Projects, ProjectDto> GetSpec() => _ => new ProjectDto
		{
			Alias = _.Alias,
			CommitToIt = _.CommitToIt,
			CommitToVcs = _.CommitToVcs,
			DefaultBranchName = _.DefaultBranchName,
			Description = _.Description,
			Id = _.Id,
			ItPluginId = _.ItPluginId,
			Name = _.DisplayName,
			SdlPolicyStatus = (SdlPolicyStatusDto)_.SdlPolicyStatus,
			VcsPluginId = _.VcsPluginId,
			VcsSyncEnabled = _.VcsSyncEnabled,
			PollTimeout = _.PollTimeout,
			EnablePoll = _.EnablePoll
		};
	}
}