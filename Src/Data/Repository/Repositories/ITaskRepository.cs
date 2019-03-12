namespace Repository.Repositories
{
	using System.Linq;

	using Common.Enums;
	using Repository.Context;

	/// <summary>
	///   Represents repository for tasks.
	/// </summary>
	public interface ITaskRepository: IWriteRepository<Tasks>
	{
		/// <summary>
		///   Gets tasks by status.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns>Tasks.</returns>
		IQueryable<Tasks> GetByStatus(TaskStatus status);

		/// <summary>
		///   Gets the last tasks in the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="count">The count of tasks to get.</param>
		/// <returns>Tasks.</returns>
		IQueryable<Tasks> GetLast(long projectId, int count);

		/// <summary>
		///   Gets the last task in the specified branch of the project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <param name="branchName">Name of the branch.</param>
		/// <returns>Task.</returns>
		Tasks GetLast(long projectId, string branchName);

		/// <summary>
		/// Gets the previous task, that was scanned before current
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		Tasks GetPrevious(Tasks task);
	}
}