namespace Repository.Context
{
	using System.ComponentModel.DataAnnotations.Schema;

	/// <summary>
	///   Represents setting.
	/// </summary>
	/// <seealso cref="Repository.IEntity"/>
	public partial class Settings: ILocalizedEntity
	{
		/// <summary>
		///   Gets the culture identifier.
		/// </summary>
		/// <value>
		///   The culture identifier.
		/// </value>
		[NotMapped]
		public long CultureId { get; }
	}
}