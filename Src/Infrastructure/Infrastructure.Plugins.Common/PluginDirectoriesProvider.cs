namespace Infrastructure.Plugins.Common
{
	using System;
	using System.Collections.Generic;
	using System.IO;

	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class PluginDirectoriesProvider : IPluginDirectoriesProvider
	{
		private readonly IPluginDirectoryNameProvider _pluginDirectoryNameProvider;

		public PluginDirectoriesProvider([NotNull] IPluginDirectoryNameProvider pluginDirectoryNameProvider)
		{
			if (pluginDirectoryNameProvider == null) throw new ArgumentNullException(nameof(pluginDirectoryNameProvider));

			_pluginDirectoryNameProvider = pluginDirectoryNameProvider;
		}

		public IEnumerable<string> GetPluginDirectories()
		{
			var parentFolderPath = _pluginDirectoryNameProvider.GetDirectory();

			return GetDirectoriesWithFiles(parentFolderPath);
		}

		private static IEnumerable<string> GetDirectoriesWithFiles([NotNull] string parentPath)
		{
			if (parentPath == null) throw new ArgumentNullException(nameof(parentPath));

			var directoryPaths = new List<string> {parentPath};

			var childs = Directory.GetDirectories(parentPath);

			foreach (var child in childs)
			{
				directoryPaths.AddRange(GetDirectoriesWithFiles(child));
			}

			return directoryPaths;
		}
	}
}