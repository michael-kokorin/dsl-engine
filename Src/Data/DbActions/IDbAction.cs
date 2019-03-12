namespace DbActions
{
	using System;
	using System.Collections.Generic;

	using DbUpdateCommon;

	/// <summary>
	///   Provides methods to execute action over database.
	/// </summary>
	public interface IDbAction
	{
		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		string Key { get; }

		/// <summary>
		///   Gets the version.
		/// </summary>
		/// <value>
		///   The version.
		/// </value>
		Version Version { get; }

		/// <summary>
		///   Executes action on the specified database.
		/// </summary>
		/// <param name="database">The database.</param>
		/// <param name="parameters">The parameters.</param>
		void Execute(IDbInformationProvider database, Dictionary<string, object> parameters);
	}
}