namespace Modules.UI.Models.Entities
{
	using System.ComponentModel.DataAnnotations;

	using Modules.UI.Resources;

	/// <summary>
	///   Represents model for scan core parameter.
	/// </summary>
	public sealed class ScanCoreModel
	{
		/// <summary>
		///   Gets or sets the display name.
		/// </summary>
		/// <value>
		///   The display name.
		/// </value>
		[Display(ResourceType = typeof(Resources), Name = "ScanCoreModel_DisplayName_Scan_core")]
		public string DisplayName { get; set; }

		/// <summary>
		///   Gets or sets the key.
		/// </summary>
		/// <value>
		///   The key.
		/// </value>
		public string Key { get; set; }
	}
}