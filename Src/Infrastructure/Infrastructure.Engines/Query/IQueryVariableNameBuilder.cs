namespace Infrastructure.Engines.Query
{
	public interface IQueryVariableNameBuilder
	{
		string Decode(string value);

		bool IsSimpleValue(string source);

		bool IsProperty(string source);

		string ToProperty(string value);

		string Encode(string source);
	}
}