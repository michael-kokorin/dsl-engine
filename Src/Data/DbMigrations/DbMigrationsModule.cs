namespace DbMigrations
{
	using JetBrains.Annotations;

	using Microsoft.Practices.Unity;

	using Common.Container;
	using Common.Extensions;
	using DbUpdateCommon;

	/// <summary>
	///     Represents container module for this project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	[UsedImplicitly]
	public sealed class DbMigrationsModule: IContainerModule
	{
		/// <summary>
		///     Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope) => container
			.RegisterType<IDbTransformationProvider, MsSqlDbTransformationProvider>(reuseScope)
			.RegisterType<IDbInformationProvider, MsSqlDbTransformationProvider>(reuseScope)
			.RegisterType<IDbExecutionProvider, MsSqlDbTransformationProvider>(reuseScope)
			.RegisterType<IDbMigrator, DbMigrator>(reuseScope);
	}
}