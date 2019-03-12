namespace Infrastructure.Engines.Query.Result
{
	public sealed class QueryResultColumn
	{
		public string Code { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public QueryResultColumn() { }

		public QueryResultColumn(string code, string name, string desc = null)
		{
			Code = code;

			Name = name;

			Description = desc;
		}
	}
}