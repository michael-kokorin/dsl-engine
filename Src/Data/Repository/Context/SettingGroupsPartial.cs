namespace Repository.Context
{
	using System.ComponentModel.DataAnnotations.Schema;

	/// <summary>
	///   Represents setting group.
	/// </summary>
	/// <seealso cref="Repository.IEntity"/>
	public partial class SettingGroups: ILocalizedEntity
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