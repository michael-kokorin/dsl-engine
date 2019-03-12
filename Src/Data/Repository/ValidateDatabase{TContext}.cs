namespace Repository
{
	using System;
	using System.Data.Entity;

	using Repository.Properties;

	/// <summary>
	///   Provides methods to validate database state.
	/// </summary>
	/// <typeparam name="TContext">The type of the context.</typeparam>
	/// <seealso cref="System.Data.Entity.IDatabaseInitializer{TContext}"/>
	internal sealed class ValidateDatabase<TContext>: IDatabaseInitializer<TContext>
		where TContext: DbContext
	{
		/// <summary>
		///   Executes the strategy to initialize the database for the given context.
		/// </summary>
		/// <param name="context">The context. </param>
		public void InitializeDatabase(TContext context)
		{
			if(!context.Database.Exists())
				throw new InvalidOperationException(Resources.DatabaseNotExists);
		}
	}
}