namespace Infrastructure.Plugins.Common
{
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure.Plugins.Common.Contracts;

	/// <summary>
	///     Loads plugins from file share
	/// </summary>
	public interface IPluginLoader<out TPlugin> where TPlugin: IPlugin
	{
		/// <summary>
		///     Loads plugins from file share.
		/// </summary>
		/// <returns>Collection of plugins.</returns>
		[NotNull]
		[ItemNotNull]
		IEnumerable<TPlugin> Load();
	}
}