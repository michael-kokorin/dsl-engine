namespace Modules.UI.Models.Entities
{
	public sealed class QueryModel
	{
		public string Comment { get; set; }

		public long Id { get; set; }

		public bool IsSystem { get; set; }

		public string Name { get; set; }

		public string Model { get; set; }

		public int Privacy { get; set; }

		public long? ProjectId { get; set; }

		public string Query { get; set; }

		public int Visibility { get; set; }
	}
}