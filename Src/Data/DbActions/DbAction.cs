namespace DbActions
{
	using System;
	using System.Collections.Generic;

	using DbUpdateCommon;

	/// <summary>
	///   Represents base action.
	/// </summary>
	/// <seealso cref="DbActions.IDbAction"/>
	public abstract class DbAction: IDbAction
	{
		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		public abstract string Key { get; }

		/// <summary>
		///   Gets the version.
		/// </summary>
		/// <value>
		///   The version.
		/// </value>
		public abstract Version Version { get; }

		/// <summary>
		///   Executes action on the specified database.
		/// </summary>
		/// <param name="database">The database.</param>
		/// <param name="parameters">The parameters.</param>
		public abstract void Execute(IDbInformationProvider database, Dictionary<string, object> parameters);
	}
}