namespace Common.Query
{
	using System;

	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	internal sealed class DataQueryHandlerProvider: IDataQueryHandlerProvider
	{
		private readonly IUnityContainer _unityContainer;

		public DataQueryHandlerProvider([NotNull] IUnityContainer unityContainer)
		{
			if(unityContainer == null) throw new ArgumentNullException(nameof(unityContainer));

			_unityContainer = unityContainer;
		}

		/// <summary>
		///   Resolves the query handler..
		/// </summary>
		/// <typeparam name="TQuery">The type of the query.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <returns>The query handler.</returns>
		public IDataQueryHandler<TQuery, TResult> Resolve<TQuery, TResult>()
			where TQuery: class, IDataQuery => _unityContainer.Resolve<IDataQueryHandler<TQuery, TResult>>();
	}
}