namespace Modules.Core.Services.UI.Renderers
{
	using System;

	using Common.Extensions;
	using Infrastructure.Plugins;
	using Modules.Core.Contracts.UI.Dto.UserSettings;

	internal sealed class PluginSettingRenderer : IDataRenderer<PluginSetting, PluginSettingDto>
	{
		public Func<PluginSetting, PluginSettingDto> GetSpec() =>
			x => new PluginSettingDto
			{
				Description = x.Description,
				DisplayName = x.DisplayName,
				PluginId = x.PluginId,
				ProjectId = x.ProjectId,
				SettingId = x.SettingId,
				ValueType = x.ValueType.GetEqualByValue<PluginSettingValueTypeDto>(),
				Value = x.Value
			};
	}
}