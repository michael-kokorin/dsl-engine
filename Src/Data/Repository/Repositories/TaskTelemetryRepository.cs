namespace Repository.Repositories
{
	using System;

	using JetBrains.Annotations;

	using Repository.Context;

	[UsedImplicitly]
	internal sealed class TaskTelemetryRepository : Repository<TaskTelemetry>, ITaskTelemetryRepository
	{
		/// <summary>
		///   Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContextProvider">The database context provider.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dbContextProvider"/> is <see langword="null"/>.</exception>
		public TaskTelemetryRepository([NotNull] IDbContextProvider dbContextProvider) : base(dbContextProvider)
		{
		}
	}
}