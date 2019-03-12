namespace Repository
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Common.Data;

	/// <summary>
	///   Represents base repository.
	/// </summary>
	/// <typeparam name="T">Type of repository entity.</typeparam>
	/// <seealso cref="Repository.IWriteRepository{T}"/>
	internal class Repository<T>: IWriteRepository<T>
		where T: class, IEntity
	{
		private readonly IDbContextProvider _dbContextProvider;

		/// <summary>
		///   Initializes a new instance of the <see cref="Repository{T}"/> class.
		/// </summary>
		/// <param name="dbContextProvider">The database context provider.</param>
		/// <exception cref="ArgumentNullException"><paramref name="dbContextProvider"/> is <see langword="null"/>.</exception>
		protected Repository([NotNull] IDbContextProvider dbContextProvider)
		{
			if(dbContextProvider == null) throw new ArgumentNullException(nameof(dbContextProvider));

			_dbContextProvider = dbContextProvider;
		}

		public virtual IQueryable<T> Query() => GetContext().Table<T>();

		/// <summary>
		///   Gets entity by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns>The entity.</returns>
		public T GetById(long id) => Query().SingleOrDefault(_ => _.Id == id);

		/// <summary>
		///   Deletes the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null"/>.</exception>
		public void Delete(T entity)
		{
			if(entity == null) throw new ArgumentNullException(nameof(entity));

			GetContext().Table<T>().Remove(entity);
		}

		/// <summary>
		///   Inserts the specified entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		/// <exception cref="ArgumentNullException"><paramref name="entity"/> is <see langword="null"/>.</exception>
		public void Insert(T entity)
		{
			if(entity == null) throw new ArgumentNullException(nameof(entity));

			GetContext().Table<T>().Add(entity);
		}

		/// <summary>
		///   Saves changes in current repository.
		/// </summary>
		public void Save() => GetContext().ApplyUpdates();

		/// <summary>
		///   Queries data.
		/// </summary>
		/// <returns>Data.</returns>
		IQueryable<object> IDataSource.Query() => Query();

		private IDbContext GetContext() => _dbContextProvider.GetContext();
	}
}