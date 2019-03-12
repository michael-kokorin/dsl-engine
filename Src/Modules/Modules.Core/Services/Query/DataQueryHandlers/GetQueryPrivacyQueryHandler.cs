namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Enums;
	using Common.Extensions;
	using Common.Query;
	using Modules.Core.Contracts.Dto;
	using Modules.Core.Properties;
	using Modules.Core.Services.Query.DataQueries;

	[UsedImplicitly]
	public sealed class GetQueryPrivacyQueryHandler : IDataQueryHandler<GetQueryPrivacyQuery, ReferenceItemDto[]>
	{
		public ReferenceItemDto[] Execute(GetQueryPrivacyQuery dataQuery)
		{
			if (dataQuery == null)
				throw new ArgumentNullException(nameof(dataQuery));

			var enumValues = Enum.GetValues(typeof(QueryPrivacyType)).Cast<QueryPrivacyType>();

			return enumValues.Select(_ => new ReferenceItemDto
			{
				Text = _.GetEnumName(Resources.ResourceManager),
				Value = (int) _
			}).ToArray();
		}
	}
}