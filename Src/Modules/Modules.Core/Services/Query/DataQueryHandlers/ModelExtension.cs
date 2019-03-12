namespace Modules.Core.Services.Query.DataQueryHandlers
{
	using Infrastructure.Query;
	using Modules.Core.Contracts.Query.Dto;

	internal static class ModelExtension
	{
		public static QueryDto ToDto(this QueryInfo query) => new QueryDto
		{
			Comment = query.Comment,
			Id = query.Id,
			IsSystem = query.IsSystem,
			Model = query.Model,
			Name = query.Name,
			Privacy = query.Privacy,
			ProjectId = query.ProjectId ?? 0,
			Query = query.Query,
			Visibility = query.Visibility
		};
	}
}