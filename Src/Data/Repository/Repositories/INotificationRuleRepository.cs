namespace Repository.Repositories
{
	using System.Linq;

	using Repository.Context;

	/// <summary>
	///   Provide methods to access notification rules.
	/// </summary>
	public interface INotificationRuleRepository: IWriteRepository<NotificationRules>
	{
		/// <summary>
		///   Gets notification rules by project and notification name.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="name">The notification rule name.</param>
		/// <returns>Notification rules.</returns>
		IQueryable<NotificationRules> Get(long projectId, string name);

		/// <summary>
		///   Gets notification rules by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns>Notification rules.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		// queryable here is required
		IQueryable<NotificationRules> GetByProject(long? projectId);
	}
}