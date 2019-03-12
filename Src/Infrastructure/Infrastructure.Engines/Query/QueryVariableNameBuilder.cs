namespace Infrastructure.Engines.Query
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	internal sealed class QueryVariableNameBuilder : IQueryVariableNameBuilder
	{
		public string ToProperty(string value) => QueryKey.ParamRegex.Replace(value, "x.$1");

		public string Decode(string value) => QueryKey.ParamRegex.Replace(value, "$1");

		public string Encode(string source) => $"#{source}#";

		public bool IsSimpleValue(string source) => QueryKey.VariableRegex.IsMatch(source);

		public bool IsProperty(string source) => QueryKey.ParamRegex.IsMatch(source);
	}
}