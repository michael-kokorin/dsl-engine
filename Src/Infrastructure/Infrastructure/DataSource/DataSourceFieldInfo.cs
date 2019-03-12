namespace Infrastructure.DataSource
{
	using JetBrains.Annotations;

	[UsedImplicitly]
	public sealed class DataSourceFieldInfo
	{
		public long DataSourceId { get; set; }

		public int DataType { get; set; }

		public string Description { get; set; }

		public long Id { get; set; }

		public string Key { get; set; }

		public string Name { get; set; }

		public long? ReferenceTableId { get; set; }
	}
}