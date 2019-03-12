namespace Infrastructure.Engines
{
	using System;

	/// <summary>
	///     Provides methods to translate entity name to type.
	/// </summary>
	public interface IQueryEntityNameTranslator
	{
		/// <summary>
		///     Gets the type of the entity.
		/// </summary>
		/// <param name="entityName">Name of the entity.</param>
		/// <returns>The type of the entity.</returns>
		Type GetEntityType(string entityName);
	}
}