// ReSharper disable MemberCanBeInternal
namespace Infrastructure.Engines
{
	using System.Text.RegularExpressions;

	public static class QueryKey
	{
		public static readonly Regex ParamRegex = new Regex("#([^#]+)#");

		public const string QueryEmptyString = "\"\"";

		public static readonly Regex VariableRegex = new Regex("^[a-zA-Z_][a-zA-Z0-9_]*$");
	}
}