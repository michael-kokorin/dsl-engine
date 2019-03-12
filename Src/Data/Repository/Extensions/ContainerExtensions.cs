namespace Repository.Extensions
{
	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Data;
	using Common.Extensions;

	/// <summary>
	///   Provides extension methods for container.
	/// </summary>
	internal static class ContainerExtensions
	{
		/// <summary>
		///   Registers the localized.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <typeparam name="TInterface">The type of the interface.</typeparam>
		/// <typeparam name="TRepository">The type of the repository.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterLocalized<TEntity, TInterface, TRepository>(this IUnityContainer container,
																																											ReuseScope reuseScope)
			where TEntity: class, ILocalizedEntity
			where TInterface: ILocalizedRepository<TEntity>
			where TRepository: class, TInterface => container
				.RegisterType<ILocalizedRepository<TEntity>, TRepository>(reuseScope)
				.RegisterRepository<TEntity, TInterface, TRepository>(reuseScope);

		/// <summary>
		///   Registers the repository.
		/// </summary>
		/// <typeparam name="TEntity">The type of the entity.</typeparam>
		/// <typeparam name="TInterface">The type of the interface.</typeparam>
		/// <typeparam name="TRepository">The type of the repository.</typeparam>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public static IUnityContainer RegisterRepository<TEntity, TInterface, TRepository>(
			this IUnityContainer container,
			ReuseScope reuseScope)
			where TEntity: class, IEntity
			where TInterface: IWriteRepository<TEntity>
			where TRepository: class, TInterface => container
				.RegisterType<IReadRepository<TEntity>, TRepository>(reuseScope)
				.RegisterType<IWriteRepository<TEntity>, TRepository>(reuseScope)
				.RegisterType<TInterface, TRepository>(reuseScope)
				.RegisterType<IDataSource<TEntity>, TRepository>(reuseScope)
				.RegisterType<IDataSource, TRepository>(typeof(TEntity).Name, reuseScope);
	}
}