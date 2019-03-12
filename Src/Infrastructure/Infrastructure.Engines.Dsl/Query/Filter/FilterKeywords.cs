namespace Infrastructure.Engines.Dsl.Query.Filter
{
	using System.ComponentModel;

	internal enum FilterKeywords
	{
		[Description(",")] ArrayDelimiter,

		[Description("[)]")] ArrayCloseTag,

		[Description("[(]")] ArrayOpenTag,

		[Description("[)]")] GroupCloseTag,

		[Description("[(]")] GroupOpenTag,

		[Description("[Ff][Aa][Ll][Ss][Ee]")] False,

		[Description("[Nn][Uu][Ll][Ll]")] Null,

		[Description("{")] ParameterOpenTag,

		[Description("}")] ParameterCloseTag,

		[Description("\"")] TextConstantCloseTag,

		[Description("\"")] TextConstantOpenTag,

		[Description("[Tt][Rr][Uu][Ee]")] True,

		[Description("#")] VariableCloseTag,

		[Description("#")] VariableOpenTag
	}
}