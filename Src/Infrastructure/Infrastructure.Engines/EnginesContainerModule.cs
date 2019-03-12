namespace Infrastructure.Engines
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Dsl.Query.Filter;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;
	using Infrastructure.Engines.Extensions;
	using Infrastructure.Engines.Query;
	using Infrastructure.Engines.Query.Filter;
	using Infrastructure.Engines.Query.Filter.Specification;

	public sealed class EnginesContainerModule : IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container

			// Query block translators
			.RegisterBlockTranslator<DslGroupBlock, GroupBlockTranslator>(reuseScope)
			.RegisterBlockTranslator<DslLimitBlock, LimitBlockTranslator>(reuseScope)
			.RegisterBlockTranslator<DslOrderBlock, OrderBlockTranslator>(reuseScope)
			.RegisterBlockTranslator<DslFormatBlock, FormatBlockTranslator>(reuseScope)
			.RegisterBlockTranslator<DslFilterBlock, FilterBlockTranslator>(reuseScope)

			// Filter query block specification translators
			.RegisterFilterSpecTranslator<FilterArraySpecification, FilterArraySpecificationTranslator>(reuseScope)
			.RegisterFilterSpecTranslator<FilterConstantSpecification, FilterConstantSpecificationTranslator>(reuseScope)
			.RegisterFilterSpecTranslator<FilterGroupSpecification, FilterGroupSpecificationTranslator>(reuseScope)
			.RegisterFilterSpecTranslator<FilterSpecification, FilterSpecificationTranslator>(reuseScope)
			.RegisterFilterSpecTranslator<FilterConditionSpecification, FilterConditionTranslator>(reuseScope)
			.RegisterFilterSpecTranslator<FilterParameterSpecification, FilterParameterSpecificationTranslator>(reuseScope)

			// Other types
			.RegisterType<IFilterSpecificationTranslatorResolver, FilterSpecificationTranslatorResolver>(reuseScope)
			.RegisterType<IFilterSpecificationTranslatorDirector, FilterSpecificationTranslatorDirector>(reuseScope)
			.RegisterType<IQueryBlockTranslatorResolver, QueryBlockTranslatorResolver>(reuseScope)
			.RegisterType<IQueryBlockTranslationManager, QueryBlockTranslationManager>(reuseScope)
			.RegisterType<IQueryTranslator, QueryTranslator>(reuseScope)
			.RegisterType<IQueryToTableRenderer, QueryToTableRenderer>(reuseScope)
			.RegisterType<IQueryBuilder, QueryBuilder>(reuseScope)
			.RegisterType<IQueryVariableNameBuilder, QueryVariableNameBuilder>(reuseScope)
			.RegisterType<IDataQueryExecutor, DataQueryExecutor>(reuseScope)
			.RegisterType<IDataQueryExpressionTranslator, DataQueryExpressionTranslator>(reuseScope)
			.RegisterType<IQueryEntityNameTranslator, QueryEntityNameTranslator>(reuseScope)
			.RegisterType<IUserProvider, UserProvider>(reuseScope)
			.RegisterType<IUserDataProvider, UserDataProvider>(reuseScope)
			.RegisterType<IWorkflowActionProvider, WorkflowActionProvider>(reuseScope);
	}
}