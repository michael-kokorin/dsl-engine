namespace Infrastructure.Engines.Query
{
	using System;
	using System.Text;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class QueryToTableRenderer : IQueryToTableRenderer
	{
		public string RenderToTable([NotNull] DslDataQuery query)
		{
			if (query == null) throw new ArgumentNullException(nameof(query));

			if (string.IsNullOrEmpty(query.TableKey) &&
				!query.IsTableRenderRequired)
				return string.Empty;

			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine(".Select(x => new QueryResultItem {");

			if (!string.IsNullOrEmpty(query.TableKey))
				stringBuilder.AppendLine("EntityId = x." + query.TableKey + ",");

			stringBuilder.AppendLine("Value = x })");

			return stringBuilder.ToString();
		}
	}
}