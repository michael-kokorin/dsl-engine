namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Data.SqlClient;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Query;
	using Modules.Core.Contracts.Dto;
	using Modules.Core.Properties;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	internal sealed class GetSortDirectionsQueryHandler : IDataQueryHandler<GetSortDirectionsQuery, ReferenceItemDto[]>
	{
		public ReferenceItemDto[] Execute(GetSortDirectionsQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var enumValues = Enum.GetValues(typeof(SortOrder)).Cast<SortOrder>();

			return enumValues.Select(_ => new ReferenceItemDto
			{
				Text = _.GetEnumName(Resources.ResourceManager),
				Value = (int)_
			}).ToArray();
		}
	}
}