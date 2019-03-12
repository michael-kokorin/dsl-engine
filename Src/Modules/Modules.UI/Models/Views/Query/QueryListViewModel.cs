namespace Modules.UI.Models.Views.Query
{
	using Modules.UI.Models.Data;

	public sealed class QueryListViewModel
	{
		public bool IsCanCreateNewQuery { get; set; }

		public TableModel Table { get; set; }
	}
}