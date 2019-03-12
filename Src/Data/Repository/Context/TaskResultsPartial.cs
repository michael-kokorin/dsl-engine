namespace Repository.Context
{
	using System.ComponentModel.DataAnnotations.Schema;

	using Common.Enums;

	/// <summary>
	///   Represents task result.
	/// </summary>
	/// <seealso cref="Repository.IEntity"/>
	[ProjectProperty("Tasks.ProjectId")]
	partial class TaskResults: IEntity
	{
		/// <summary>
		///   Gets or sets the severity type information.
		/// </summary>
		/// <value>
		///   The severity type information.
		/// </value>
		/// <remarks>DO NOT USE IN QUERIES. USE <see cref="SeverityType"/> INSTEAD.</remarks>
		[NotMapped]
		public VulnerabilitySeverityType SeverityTypeInfo
		{
			get { return (VulnerabilitySeverityType)SeverityType; }
			set { SeverityType = (int)value; }
		}
	}
}