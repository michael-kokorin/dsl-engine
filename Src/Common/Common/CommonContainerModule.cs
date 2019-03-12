namespace Common
{
	using Microsoft.Practices.Unity;

	using Common.Command;
	using Common.Container;
	using Common.Data;
	using Common.Extensions;
	using Common.FileSystem;
	using Common.Html;
	using Common.Licencing;
	using Common.Logging;
	using Common.Query;
	using Common.Settings;
	using Common.SystemComponents;
	using Common.Time;

	/// <summary>
	///   Represents container module for the project.
	/// </summary>
	/// <seealso cref="Common.Container.IContainerModule"/>
	public sealed class CommonContainerModule : IContainerModule
	{
		/// <summary>
		///   Registers binding to the specified container.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="reuseScope">The reuse scope.</param>
		/// <returns>The container.</returns>
		public IUnityContainer Register(IUnityContainer container, ReuseScope reuseScope)
		{
			container
				.RegisterType<IFileWriter, FileWriter>(reuseScope)
				.RegisterType<ITimeService, DateTimeService>(reuseScope)
				.RegisterType<ILog, NLogLog>(new ContainerControlledLifetimeManager())
				.RegisterType<ICommandHandlerProvider, ContainerCommandHandlerProvider>(reuseScope)
				.RegisterType<ICommandDispatcher, CommandDispatcher>(reuseScope)
				.RegisterType<IDataQueryHandlerProvider, DataQueryHandlerProvider>(reuseScope)
				.RegisterType<IDataQueryDispatcher, DataQueryDispatcher>(reuseScope)
				.RegisterType<IConfigManager, AppConfigManager>(reuseScope)
				.RegisterType<IDataSourceProvider, DataSourceProvider>(reuseScope)
				.RegisterType<IFileSystemInfoProvider, FileSystemInfoProvider>(reuseScope)
				.RegisterType<ISystemVersionProvider, AssemblySystemVersionProvider>(reuseScope)
				.RegisterType<IHtmlEncoder, HtmlEncoder>(reuseScope)

				.RegisterType<IInstallationLicenceIdProvider, ConfigInstallationLicenceIdProvider>(reuseScope)
				.RegisterType<ILicenceProvider, LicenceProvider>(reuseScope);

			return container;
		}
	}
}