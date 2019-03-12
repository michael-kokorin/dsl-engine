namespace Modules.Core.Services.Query.DataQueries
{
	using Common.Query;

	public sealed class GetRolesByUserQuery : IDataQuery
	{
		public readonly long UserId;

		public GetRolesByUserQuery(long userId)
		{
			UserId = userId;
		}
	}
}