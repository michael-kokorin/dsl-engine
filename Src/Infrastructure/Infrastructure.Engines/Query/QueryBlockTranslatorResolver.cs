namespace Infrastructure.Engines.Query
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class QueryBlockTranslatorResolver : IQueryBlockTranslatorResolver
	{
		private readonly IUnityContainer _unityContainer;

		public QueryBlockTranslatorResolver([NotNull] IUnityContainer unityContainer)
		{
			if (unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		public IQueryBlockTranslator<T> Resolve<T>() where T : class, IDslQueryBlock
			=> _unityContainer.Resolve<IQueryBlockTranslator<T>>();
	}
}