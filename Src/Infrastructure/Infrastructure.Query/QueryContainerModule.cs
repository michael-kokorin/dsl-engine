namespace Infrastructure.Query
{
	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using Infrastructure.Query.Evaluation;

	[UsedImplicitly]
	public sealed class QueryContainerModule: IContainerModule
	{
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IQueryAccessValidator, QueryAccessValidator>(reuseScope)
			.RegisterType<IQueryStorage, QueryStorage>(reuseScope)
			.RegisterType<IQueryExecutor, QueryExecutor>(reuseScope)
			.RegisterType<IDslDataQueryEvaluator, DslDataQueryEvaluator>(reuseScope)
			.RegisterType<IDslDataQueryEvaluator, DslDataQueryEvaluator>(reuseScope)
			.RegisterType<IQueryModelProcessor, QueryModelProcessor>(reuseScope)
			.RegisterType<IQueryModelAccessValidator, QueryModelAccessValidator>(reuseScope)
			.RegisterType<IFormatBlockValueAccessEvaluator, FormatBlockValueAccessEvaluator>(reuseScope)
			.RegisterType<IQueryProjectRestrictor, QueryProjectRestrictor>(reuseScope)
			.RegisterType<IQueryEntityNamePropertyTypeNameResolver, QueryEntityNamePropertyTypeNameResolver>(reuseScope)
			.RegisterType<IQueryModelValidator, QueryModelValidator>(reuseScope);
	}
}