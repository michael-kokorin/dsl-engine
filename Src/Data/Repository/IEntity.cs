namespace Repository
{
	/// <summary>
	///   Represents an entity.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		///   Gets the identifier.
		/// </summary>
		/// <value>
		///   The identifier.
		/// </value>
		long Id { get; }
	}
}