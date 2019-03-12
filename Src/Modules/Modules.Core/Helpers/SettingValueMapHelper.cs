namespace Modules.Core.Helpers
{
	using System.Collections.Generic;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Infrastructure.Plugins.Common.Contracts;
	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	internal static class SettingValueMapHelper
	{
		[CanBeNull]
		public static string GetDefaultValue([NotNull] this Settings settings) =>
			settings.SettingType != (int)SettingType.List
				? settings.DefaultValue
				: new SettingValueDto
					{
						DefaultValue = settings.DefaultValue,
						Type = ((SettingType)settings.SettingType).GetEqualByValue<SettingValueTypeDto>()
					}.GetSubitems().FirstOrDefault()?.Key;

		[NotNull]
		public static SettingValueDto GetDefaultValueDto([NotNull] this Settings setting) =>
			new SettingValueDto
			{
				Title = setting.DisplayName,
				ChildGroups = new List<SettingGroupDto>(),
				DefaultValue = setting.DefaultValue,
				Type = ((SettingType)setting.SettingType).GetEqualByValue<SettingValueTypeDto>()
			};

		[NotNull]
		public static SettingValueDto ToDto([NotNull] this SettingValues settingValues) =>
			new SettingValueDto
			{
				Id = settingValues.Id,
				Title = settingValues.Settings.DisplayName,
				Value = settingValues.Value,
				Type = (SettingValueTypeDto)settingValues.Settings.SettingType,
				DefaultValue = settingValues.Settings.DefaultValue,
				ChildGroups = new List<SettingGroupDto>()
			};
	}
}