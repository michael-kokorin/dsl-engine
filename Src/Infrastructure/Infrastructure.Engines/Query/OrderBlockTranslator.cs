namespace Infrastructure.Engines.Query
{
	using System;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Text;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class OrderBlockTranslator : IQueryBlockTranslator<DslOrderBlock>
	{
		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		public OrderBlockTranslator([NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
		{
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));

			_queryVariableNameBuilder = queryVariableNameBuilder;
		}

		public string Translate([NotNull] DslOrderBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var first = queryBlock.Items.First();

			var stringBUilder = new StringBuilder();

			stringBUilder.AppendLine("." + (first.SortOrder == SortOrder.Descending ? "OrderByDescending" : "OrderBy") +
			                         "(x => x." +
			                         first.OrderFieldName + ")");

			foreach (var orderItem in queryBlock.Items.Skip(1))
			{
				stringBUilder.AppendLine("." + (orderItem.SortOrder == SortOrder.Descending ? "ThenByDescending" : "ThenBy") +
				                         "(x => x." +
				                         orderItem.OrderFieldName + ")");
			}

			return stringBUilder.ToString();
		}

		public string ToDsl([NotNull] DslOrderBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			if (queryBlock.Items == null ||
			    !queryBlock.Items.Any())
				return string.Empty;

			var orderFieds = string.Join(", ",
				queryBlock.Items
					.Select(_ =>
						$"{_queryVariableNameBuilder.Encode(_.OrderFieldName)} {(_.SortOrder == SortOrder.Ascending ? DslKeywords.Asc : DslKeywords.Desc)}"));

			return $"{DslKeywords.Order} {orderFieds}";
		}
	}
}