namespace Infrastructure.Query.Extensions
{
	using Repository.Context;

	internal static class EntityExtension
	{
		public static QueryInfo ToDto(this Queries query) => new QueryInfo
		{
			Comment = query.Comment,
			Id = query.Id,
			IsSystem = query.IsSystem,
			Model = query.JsonQuery,
			Name = query.Name,
			Privacy = query.Privacy,
			ProjectId = query.ProjectId,
			Query = query.Query,
			Visibility = query.Visibility
		};
	}
}
