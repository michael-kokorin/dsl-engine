namespace Infrastructure.Plugins
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	using Infrastructure.Plugins.Common.Contracts;
	using Repository.Context;

	/// <summary>
	///     Initializes plugin information in database
	/// </summary>
	public interface IPluginProvider
	{
		/// <summary>
		///     Initializes the specified plugin.
		/// </summary>
		/// <param name="plugin">The plugin instance to initialize.</param>
		void Initialize(IPlugin plugin);

		/// <summary>
		/// Gets plugins list.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Plugins> Get(Expression<Func<Plugins, bool>> specification);
	}
}