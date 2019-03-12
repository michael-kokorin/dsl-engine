namespace Modules.UI.Models.Views.Query
{
	using Modules.UI.Models.Entities;

	public sealed class QueryViewModel
	{
		public ReferenceItemModel[] AccessReference { get; set; }

		public long CurrentUserId { get; set; }

		public bool IsCanEdit { get; set; }

		public QueryModel Query { get; set; }

		public ReferenceItemModel[] SortOrderDirections { get; set; }

		public UserModel[] Users { get; set; }
	}
}