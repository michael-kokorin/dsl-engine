namespace Infrastructure.Engines.Query.Filter
{
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	public interface IFilterSpecificationTranslator<in T>
		where T : class, IFilterSpecification
	{
		string Translate(T specification);

		string ToDsl(T specification);
	}
}