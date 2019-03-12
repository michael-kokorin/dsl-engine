namespace Infrastructure.RequestHandling
{
	using System.Collections.Generic;
	using System.IO;

	using Common.Extensions;

	public sealed class SolutionFileParameterTranslator: ISAParameterTranslator
	{
		private Dictionary<string, string> _parameters;

		/// <inheritdoc />
		public string Key => "sln-path";

		/// <inheritdoc />
		public void Initialize(Dictionary<string, string> parameters) => _parameters = parameters;

		/// <inheritdoc />
		public string Translate(string value)
		{
			if(string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			var folderPath = _parameters.Get("FolderPath");
			return "--solution-file \"" + Path.Combine(folderPath, value) + "\"";
		}
	}
}