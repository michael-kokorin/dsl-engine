namespace Modules.Core.Services.UI.Queries
{
	using Common.Query;

	internal sealed class GetNotificationQuery : IDataQuery
	{
		public long Id { get; set; }

		public GetNotificationQuery(long id)
		{
			Id = id;
		}
	}
}