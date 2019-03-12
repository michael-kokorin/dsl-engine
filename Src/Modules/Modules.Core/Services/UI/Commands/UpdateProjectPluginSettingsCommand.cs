namespace Modules.Core.Services.UI.Commands
{
	using System.Collections.Generic;

	using Common.Command;
	using Infrastructure.Plugins;

	internal sealed class UpdateProjectPluginSettingsCommand : ICommand
	{
		public long ProjectId { get; set; }

		public long PluginId { get; set; }

		public IEnumerable<ProjectPluginSetting> Settings { get; set; }
	}
}