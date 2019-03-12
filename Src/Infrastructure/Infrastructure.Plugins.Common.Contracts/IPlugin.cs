namespace Infrastructure.Plugins.Common.Contracts
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition;

	using JetBrains.Annotations;

	/// <summary>
	///     Plugin base interface
	/// </summary>
	[InheritedExport(typeof(IPlugin))]
	public interface IPlugin
	{
		/// <summary>
		///     Gets the plugin title
		/// </summary>
		/// <value>Plugin title</value>
		[NotNull]
		string Title { get; }

		/// <summary>
		///     Gets the plugin settings
		/// </summary>
		/// <returns>Settings</returns>
		[NotNull]
		PluginSettingGroupDefinition GetSettings();

		/// <summary>
		///     Initialize plugin by settings
		/// </summary>
		/// <param name="values">The setting values</param>
		void LoadSettingValues([NotNull] IDictionary<string, string> values);
	}
}