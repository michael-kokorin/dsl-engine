namespace Modules.Core.Services.UI
{
	using Common.Security;
	using Infrastructure.Plugins;
	using Infrastructure.Plugins.Contracts;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Contracts.UI.Dto.ProjectSettings;

	internal static class DtoExtension
	{
		public static UserDto ToDto(this UserInfo userInfo)
		{
			if (userInfo == null)
				return null;

			return new UserDto
			{
				DisplayName = userInfo.DisplayName,
				Email = userInfo.Email,
				Id = userInfo.Id,
				Login = userInfo.Login,
				Sid = userInfo.Sid
			};
		}

		public static BranchDto ToDto(this BranchInfo branch) => new BranchDto
		{
			Id = branch.Id,
			IsDefault = branch.IsDefault,
			Name = branch.IsDefault ? $"{branch.Name}*" : branch.Name
		};

		public static ProjectPluginSetting ToEntity(this UpdateProjectPluginSettingDto dto) => new ProjectPluginSetting
		{
			SettingId = dto.SettingId,
			ProjectId = dto.ProjectId,
			Value = dto.Value
		};
	}
}