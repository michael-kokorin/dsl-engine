namespace Common.Query
{
	using System;

	using JetBrains.Annotations;

	internal sealed class DataQueryDispatcher: IDataQueryDispatcher
	{
		private readonly IDataQueryHandlerProvider _dataQueryHandlerProvider;

		public DataQueryDispatcher([NotNull] IDataQueryHandlerProvider dataQueryHandlerProvider)
		{
			if(dataQueryHandlerProvider == null) throw new ArgumentNullException(nameof(dataQueryHandlerProvider));

			_dataQueryHandlerProvider = dataQueryHandlerProvider;
		}

		/// <summary>
		///   Processes the specified query.
		/// </summary>
		/// <typeparam name="TQuery">The type of the query.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="query">The query.</param>
		/// <returns>The result of processing.</returns>
		public TResult Process<TQuery, TResult>(TQuery query) where TQuery: class, IDataQuery
		{
			if(query == null)
				throw new ArgumentNullException(nameof(query));

			var queryHandler = _dataQueryHandlerProvider.Resolve<TQuery, TResult>();

			if(queryHandler == null)
				throw new UnknownDataQueryException();

			return queryHandler.Execute(query);
		}
	}
}