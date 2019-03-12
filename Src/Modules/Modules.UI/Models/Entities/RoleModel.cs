namespace Modules.UI.Models.Entities
{
	public sealed class RoleModel
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public long? ProjectId { get; set; }
	}
}