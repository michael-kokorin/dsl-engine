namespace Infrastructure.Engines.Query.Filter
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	internal sealed class FilterSpecificationTranslatorResolver : IFilterSpecificationTranslatorResolver
	{
		private readonly IUnityContainer _unityContainer;

		public FilterSpecificationTranslatorResolver([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IFilterSpecificationTranslator<T> Resolve<T>([NotNull] T specification) where T : class, IFilterSpecification
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			return _unityContainer.Resolve<IFilterSpecificationTranslator<T>>();
		}
	}
}