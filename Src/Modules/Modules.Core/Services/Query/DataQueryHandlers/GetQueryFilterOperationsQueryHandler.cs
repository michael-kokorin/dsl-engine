namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Extensions;
	using Common.Query;
	using Infrastructure.Engines.Dsl.Query.Filter;
	using Modules.Core.Contracts.Dto;
	using Modules.Core.Properties;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	internal sealed class GetQueryFilterOperationsQueryHandler : IDataQueryHandler<GetQueryFilterOperationsQuery, ReferenceItemDto[]>
	{
		public ReferenceItemDto[] Execute(GetQueryFilterOperationsQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var enumValues = Enum.GetValues(typeof(FilterOperator)).Cast<FilterOperator>();

			return enumValues.Select(_ => new ReferenceItemDto
			{
				Text = _.GetEnumName(Resources.ResourceManager) ?? _.ToString(),
				Value = (int)_
			}).ToArray();
		}
	}
}