namespace Modules.Core.Services.UI.Renderers
{
	using System;

	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	internal sealed class TaskResultRenderer : IDataRenderer<TaskResults, TaskResultDto>
	{
		public Func<TaskResults, TaskResultDto> GetSpec() => result =>
			new TaskResultDto
			{
				AdditionalExploitConditions = result.AdditionalExploitConditions,
				Description = result.Description,
				ExploitGraph = result.ExploitGraph,
				File = result.File,
				Function = result.Function,
				Id = result.Id,
				IssueNumber = result.IssueNumber,
				IssueUrl = result.IssueUrl,
				LineNumber = result.LineNumber,
				LinePosition = result.LinePosition,
				Message = result.Message,
				Place = result.Place,
				ProjectId = result.Tasks.Projects.Id,
				Rawline = result.RawLine,
				SeverityType = result.SeverityType,
				SourceFile = result.SourceFile,
				TaskId = result.TaskId,
				Type = result.Type,
				TypeShort = result.TypeShort
			};
	}
}