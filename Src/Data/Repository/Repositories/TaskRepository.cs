namespace Repository.Repositories
{
	using System.Linq;

	using Common.Enums;
	using Repository.Context;

	internal sealed class TaskRepository : Repository<Tasks>, ITaskRepository
	{
		public TaskRepository(IDbContextProvider dbContextProvider) : base(dbContextProvider)
		{
		}

		/// <summary>
		///   Gets tasks by status.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns>Tasks.</returns>
		public IQueryable<Tasks> GetByStatus(TaskStatus status) => Query().Where(_ => _.Status == (int) status);

		public IQueryable<Tasks> GetLast(long projectId, int count) =>
			Query()
				.Where(_ => _.ProjectId == projectId)
				.OrderByDescending(_ => _.Created)
				.Take(count);

		/// <summary>
		///   Gets the last task in the specified branch of the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="branchName">Name of the branch.</param>
		/// <returns>Task.</returns>
		public Tasks GetLast(long projectId, string branchName) =>
			Query()
				.Where(_ => (_.ProjectId == projectId) && (_.Repository == branchName))
				.OrderByDescending(_ => _.Finished)
				.FirstOrDefault();

		/// <summary>
		/// Gets the previous task, that was scanned before current
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		public Tasks GetPrevious(Tasks task) => Query()
			.Where(x => x.ProjectId == task.ProjectId &&
			            x.Finished.HasValue &&
			            x.Id != task.Id &&
			            x.Finished.Value < task.Finished.Value &&
			            x.Repository == task.Repository)
			.OrderByDescending(x => x.Finished.Value)
			.FirstOrDefault();
	}
}