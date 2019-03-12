namespace DbActions.ActionsStore
{
	using System;
	using System.Collections.Generic;

	using DbUpdateCommon;

	internal sealed class AddProjectAction: DbAction
	{
		/// <summary>
		///   Gets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		public override string Key => "AddTemplateProject";

		/// <summary>
		///   Gets the version.
		/// </summary>
		/// <value>
		///   The version.
		/// </value>
		public override Version Version => new Version(1, 0, 0, 0);

		/// <summary>
		///   Executes action on the specified database.
		/// </summary>
		/// <param name="database">The database.</param>
		/// <param name="parameters">The parameters.</param>
		public override void Execute(IDbInformationProvider database, Dictionary<string, object> parameters)
		{
		}
	}
}