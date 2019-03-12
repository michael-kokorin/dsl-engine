namespace Infrastructure.Engines.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;
	using Infrastructure.Engines.Query;
	using Infrastructure.Engines.Query.Filter;

	internal static class ContainerExtension
	{
		public static IUnityContainer RegisterBlockTranslator<TBlock, TTranslator>(this IUnityContainer container,
			ReuseScope reuseScope)
			where TBlock : IDslQueryBlock
			where TTranslator : IQueryBlockTranslator<TBlock> =>
				container.RegisterType<IQueryBlockTranslator<TBlock>, TTranslator>(reuseScope);

		public static IUnityContainer RegisterFilterSpecTranslator<TSpecification, TTranslator>(
			this IUnityContainer container,
			ReuseScope reuseScope)
			where TSpecification : class, IFilterSpecification
			where TTranslator : class, IFilterSpecificationTranslator<TSpecification> =>
				container.RegisterType<IFilterSpecificationTranslator<TSpecification>, TTranslator>(reuseScope);
	}
}