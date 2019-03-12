namespace DbActions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Microsoft.Practices.Unity;

	internal sealed class DbActionProvider: IDbActionProvider
	{
		public DbActionProvider(IUnityContainer container)
		{
			Container = container;
		}

		private IUnityContainer Container { get; }

		/// <summary>
		///   Gets action with the specified key and version.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="version">The version.</param>
		/// <returns></returns>
		public IDbAction Get(string key, Version version) =>
			GetAll().FirstOrDefault(x => (x.Key == key) && (x.Version == version));

		/// <summary>
		///   Gets all actions.
		/// </summary>
		/// <returns>Collection with all actions.</returns>
		public IEnumerable<IDbAction> GetAll() => Container.ResolveAll<IDbAction>();
	}
}