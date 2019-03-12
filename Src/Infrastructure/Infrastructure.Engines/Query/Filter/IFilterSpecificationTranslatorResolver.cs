namespace Infrastructure.Engines.Query.Filter
{
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	public interface IFilterSpecificationTranslatorResolver
	{
		IFilterSpecificationTranslator<T> Resolve<T>(T specification) where T : class, IFilterSpecification;
	}
}