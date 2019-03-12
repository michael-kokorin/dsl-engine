namespace Infrastructure.Plugins.Common
{
	/// <summary>
	///     Initializes plugins environment
	/// </summary>
	public interface IPluginInitializer
	{
		/// <summary>
		///     Initializes this instance.
		/// </summary>
		void InitializePlugins();
	}
}