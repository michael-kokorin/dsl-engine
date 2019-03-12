// ReSharper disable MemberCanBeInternal
namespace Infrastructure.Engines.Dsl
{
	/// <summary>
	/// List of DSL keywords
	/// 
	/// NOTE: please, add new values in alphabet order
	/// </summary>
	public static class DslKeywords
	{
		public const string Asc = "asc";

		public const string Attach = "attach";

		public const string CommentLine = "//";

		public const string Desc = "desc";

		public const char DisplayNameDelimiter = '@';

		public const string First = "first";

		public const string FirstOrDefault = "firstordefault";

		public const string Format = "format";

		public const string Group = "group";

		public const string Order = "order";

		public const string Parameters = "parameters";

		public const string Protocol = "protocol";

		public const string RepeatTimeTrigger = "repeat";

		public const string Report = "report";

		public const string Select = "select";

		public const string SelectEnd = "select end";

		public const string Skip = "skip";

		public const string Table = "table";

		public const string Take = "take";

		public const string TimeTrigger = "trigger";

		public const string Where = "where";
	}
}