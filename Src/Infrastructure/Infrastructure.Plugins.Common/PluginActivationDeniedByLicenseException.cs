namespace Infrastructure.Plugins.Common
{
	using System;

	/// <summary>
	///     Provides information about error when plugin is not available because of license.
	/// </summary>
	/// <seealso cref="System.Exception"/>
	public sealed class PluginActivationDeniedByLicenseException: Exception
	{
		public PluginActivationDeniedByLicenseException(string licenseId, string pluginType)
			: base($"Plugin activation denied by license. License Id='{licenseId}', Plugin type='{pluginType}'")
		{
		}
	}
}