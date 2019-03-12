namespace Infrastructure.Engines
{
	using Infrastructure.Engines.Dsl.Query;

	/// <summary>
	///     Provides method to translate expression to data query.
	/// </summary>
	public interface IDataQueryExpressionTranslator
	{
		/// <summary>
		///     Translates the specified query expression.
		/// </summary>
		/// <param name="queryExpression">The query expression.</param>
		/// <returns>The translated query.</returns>
		string Translate(DslDataQuery queryExpression);
	}
}