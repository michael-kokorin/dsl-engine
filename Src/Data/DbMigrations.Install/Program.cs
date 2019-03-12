namespace DbMigrations.Install
{
	using System;

	using Microsoft.Practices.Unity;

	using Common;
	using Common.Container;
	using Common.Extensions;
	using Common.Logging;
	using DbActions;
	using DbUpdateCommon;
	using Packages;

	using static Properties.Resources;

	/// <summary>
	///   Contains entry point of application.
	/// </summary>
	internal static class Program
	{
		/// <summary>
		///   Defines the entry point of the application.
		/// </summary>
		private static void Main()
		{
			IoC.InitDefault(
				ReuseScope.Container,
				new DbActionsModule(),
				new CommonContainerModule(),
				new DbUpdateCommonModule(),
				new DbMigrationsModule(),
				new PackagesContainerModule());

			var logger = IoC.GetContainer().Resolve<ILog>();

			try
			{
				var migrator = IoC.GetContainer().Resolve<IDbMigrator>();
				migrator.MigrationLatest();

				Console.WriteLine(FinishMessage);
			}
			catch(Exception exception)
			{
				Console.WriteLine(exception.Format());
				logger.Fatal(exception.Format());
			}

			Console.ReadLine();
		}
	}
}