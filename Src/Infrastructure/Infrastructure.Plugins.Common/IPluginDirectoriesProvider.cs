namespace Infrastructure.Plugins.Common
{
	using System.Collections.Generic;

	public interface IPluginDirectoriesProvider
	{
		IEnumerable<string> GetPluginDirectories();
	}
}