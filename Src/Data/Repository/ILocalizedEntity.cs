namespace Repository
{
	using JetBrains.Annotations;

	/// <summary>
	///   Represents an entity that supports localization.
	/// </summary>
	/// <seealso cref="Repository.IEntity"/>
	public interface ILocalizedEntity: IEntity
	{
		/// <summary>
		///   Gets the culture identifier.
		/// </summary>
		/// <value>
		///   The culture identifier.
		/// </value>
		[UsedImplicitly]
		long CultureId { get; }
	}
}