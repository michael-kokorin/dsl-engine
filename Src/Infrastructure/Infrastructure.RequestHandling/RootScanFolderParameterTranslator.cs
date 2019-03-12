namespace Infrastructure.RequestHandling
{
	using System.Collections.Generic;
	using System.IO;

	using Common.Extensions;

	public sealed class RootScanFolderParameterTranslator: ISAParameterTranslator
	{
		private Dictionary<string, string> _parameters;

		/// <inheritdoc/>
		public string Key => "root-scan-folder";

		/// <inheritdoc/>
		public void Initialize(Dictionary<string, string> parameters) => _parameters = parameters;

		/// <inheritdoc/>
		public string Translate(string value)
		{
			var folderPath = _parameters.Get("FolderPath");
			if(!string.IsNullOrWhiteSpace(value))
			{
				folderPath = Path.Combine(folderPath, value);
			}

			return $"-d \"{folderPath}\"";
		}
	}
}