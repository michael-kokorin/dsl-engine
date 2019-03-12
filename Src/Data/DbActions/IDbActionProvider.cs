namespace DbActions
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	///   Provides methods to get database actions.
	/// </summary>
	public interface IDbActionProvider
	{
		/// <summary>
		///   Gets action with the specified key and version.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="version">The version.</param>
		/// <returns></returns>
		IDbAction Get(string key, Version version);

		/// <summary>
		///   Gets all actions.
		/// </summary>
		/// <returns>Collection with all actions.</returns>
		IEnumerable<IDbAction> GetAll();
	}
}