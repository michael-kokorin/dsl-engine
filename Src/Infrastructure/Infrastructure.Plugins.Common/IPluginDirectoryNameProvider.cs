namespace Infrastructure.Plugins.Common
{
	using JetBrains.Annotations;

	/// <summary>
	///     Provides plugin directory name
	/// </summary>
	public interface IPluginDirectoryNameProvider
	{
		/// <summary>
		///     Gets the plugins directory.
		/// </summary>
		/// <returns>Plugins directory path</returns>
		[NotNull]
		string GetDirectory(bool writeToConfig = false);
	}
}