namespace Modules.Core.Services.Query.DataQueries
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;

	[UsedImplicitly]
	public sealed class GetQueryTextQuery : IDataQuery
	{
		public readonly string QueryModel;

		public GetQueryTextQuery([NotNull] string queryModel)
		{
			if (queryModel == null) throw new ArgumentNullException(nameof(queryModel));

			QueryModel = queryModel;
		}
	}
}