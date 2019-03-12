namespace Modules.UI.Models.Entities
{
	using System;
	using System.Web;

	using Common.Extensions;
	using Modules.Core.Contracts.Query.Dto;
	using Modules.Core.Contracts.UI.Dto;

	internal static class ModelExtension
	{
		public static UserModel ToModel(this UserDto user)
		{
			if(user == null)
				return null;

			return new UserModel
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Id = user.Id,
				Login = user.Login,
				Sid = user.Sid
			};
		}

		public static QueryModel ToModel(this QueryDto queryDto) => new QueryModel
		{
			Comment = queryDto.Comment,
			Id = queryDto.Id,
			IsSystem = queryDto.IsSystem,
			Model = queryDto.Model,
			Name = queryDto.Name,
			Privacy = queryDto.Privacy,
			ProjectId = queryDto.ProjectId,
			Query = queryDto.Query,
			Visibility = queryDto.Visibility
		};

		public static QueryDto ToDto(this QueryModel queryModel) => new QueryDto
		{
			Comment = queryModel.Comment,
			Id = queryModel.Id,
			Model = queryModel.Model,
			Name = queryModel.Name,
			Privacy = queryModel.Privacy,
			Query = queryModel.Query,
			Visibility = queryModel.Visibility
		};

		public static RoleModel ToModel(this UserRoleDto role) =>
			new RoleModel
			{
				Id = role.Id,
				Name = role.DisplayName,
				ProjectId = role.ProjectId
			};

		public static ProjectModel ToModel(this ProjectDto project)
		{
			if (project == null)
				return null;

			return new ProjectModel
			{
				Alias = project.Alias,
				CommitToIt = project.CommitToIt,
				CommitToVcs = project.CommitToVcs,
				DefaultBranchName = project.DefaultBranchName,
				Description = HttpUtility.HtmlDecode(project.Description),
				Id = project.Id,
				ItPluginId = project.ItPluginId,
				Name = project.Name,
				SdlPolicyStatus = project.SdlPolicyStatus.ToModel(),
				VcsPluginId = project.VcsPluginId,
				VcsSyncEnabled = project.VcsSyncEnabled,
				PollTimeout = project.PollTimeout,
				EnablePoll = project.EnablePoll
			};
		}

		private static SdlPolicyStatusModel ToModel(this SdlPolicyStatusDto status)
		{
			switch (status)
			{
				case SdlPolicyStatusDto.Failed:
					return SdlPolicyStatusModel.Failed;
				case SdlPolicyStatusDto.Success:
					return SdlPolicyStatusModel.Success;
				case SdlPolicyStatusDto.Unknown:
					return SdlPolicyStatusModel.Unknown;
					case SdlPolicyStatusDto.Error:
						return SdlPolicyStatusModel.Error;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static ScanCoreModel ToModel(this ScanCoreDto scanCore)
		{
			if(scanCore == null)
				return null;

			return new ScanCoreModel
			{
				DisplayName = scanCore.DisplayName,
				Key = scanCore.Key
			};
		}

		public static TaskModel ToModel(this TaskDto task)
		{
			if(task == null)
				return null;

			var model = new TaskModel
			{
				Created = task.Created,
				CreatedBy = task.CreatedBy,
				Finished = task.Finished,
				FolderPath = task.FolderPath,
				FolderSizeKb = task.FolderSizeKb,
				Id = task.Id,
				ItPluginId = task.ItPluginId,
				ProjectId = task.ProjectId,
				Repository = task.Repository,
				ScanCoreWorkingTimeSec = task.ScanCoreWorkTimeSec,
				SdlPolicyStatus = task.SdlPolicyStatus.ToModel(),
				Status = task.Status.GetEqualByValue<TaskStatusModel>(),
				VcsPluginId = task.VcsPluginId
			};


			return model;
		}

		public static TaskResultModel ToModel(this TaskResultDto result)
		{
			if(result == null)
				return null;

			return new TaskResultModel
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
				Rawline = result.Rawline,
				SeverityType = result.SeverityType,
				SourceFile = result.SourceFile,
				TaskId = result.TaskId,
				Type = result.Type,
				TypeShort = result.TypeShort
			};
		}
	}
}