namespace Infrastructure.Engines.Query.Filter
{
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	public interface IFilterSpecificationTranslatorDirector
	{
		string Translate<T>(T specification) where T : class, IFilterSpecification;

		string ToDsl<T>(T specification) where T : class, IFilterSpecification;
	}
}