namespace Infrastructure.Query
{
	using System;
	using System.Collections.Generic;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query;

	internal sealed class QueryModelValidator: IQueryModelValidator
	{
		public void Validate([NotNull] DslDataQuery dslDataQuery)
		{
			if (dslDataQuery == null) throw new ArgumentNullException(nameof(dslDataQuery));

			RemoveEmptyFormatItems(dslDataQuery);
		}

		private static void RemoveEmptyFormatItems(DslDataQuery dslDataQuery)
		{
			foreach (var queryBlock in dslDataQuery.Blocks)
			{
				// ReSharper disable once InvertIf
				if (queryBlock is DslFormatBlock)
				{
					var formatBlock = queryBlock as DslFormatBlock;

					var selectItemsList = new List<DslFormatItem>();

					// ReSharper disable once LoopCanBeConvertedToQuery
					foreach (var selectItem in formatBlock.Selects)
					{
						if (!string.IsNullOrEmpty(selectItem.Value))
						{
							selectItemsList.Add(selectItem);
						}
					}

					formatBlock.Selects = selectItemsList.ToArray();
				}
			}
		}
	}
}