namespace Repository.Repositories
{
	using System.Linq;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class NotificationRuleRepository: Repository<NotificationRules>, INotificationRuleRepository
	{
		public NotificationRuleRepository(IDbContextProvider dbContextProvider): base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets notification rules by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>
		///   Notification rules.
		/// </returns>
		public IQueryable<NotificationRules> GetByProject(long? projectId)
			=> Query().Where(_ => projectId == null || _.ProjectId == projectId);

		/// <summary>
		///   Gets notification rules by project and notification name.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The notification rule name.</param>
		/// <returns>
		///   Notification rules.
		/// </returns>
		public IQueryable<NotificationRules> Get(long projectId, string name) =>
			Query().Where(_ => (_.ProjectId == projectId) && (_.DisplayName == name));
	}
}