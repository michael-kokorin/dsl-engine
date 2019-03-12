namespace Infrastructure.Engines.Query
{
	using System;
	using System.Text;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class FormatBlockTranslator : IQueryBlockTranslator<DslFormatBlock>
	{
		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		public FormatBlockTranslator([NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
		{
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));

			_queryVariableNameBuilder = queryVariableNameBuilder;
		}

		public string Translate([NotNull] DslFormatBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var stringBUilder = new StringBuilder();

			stringBUilder.Append(".AsEnumerable().Select(x => new {");

			foreach (var selectItemExpr in queryBlock.Selects)
			{
				var value = _queryVariableNameBuilder.ToProperty(selectItemExpr.Value);

				var nameSpecified = !string.IsNullOrWhiteSpace(selectItemExpr.Name);

				stringBUilder.Append(nameSpecified
					? $"{selectItemExpr.Name}={value},"
					: $"{value},");
			}

			stringBUilder.AppendLine("})");

			return stringBUilder.ToString();
		}

		public string ToDsl([NotNull] DslFormatBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine(DslKeywords.Select);

			foreach (var selectItem in queryBlock.Selects)
			{
				var nameSpecified = !string.IsNullOrWhiteSpace(selectItem.Name);

				if (nameSpecified)
				{
					stringBuilder.Append($"{selectItem.Name}=");
				}

				stringBuilder.Append(selectItem.Value);

				var descSpecified = !string.IsNullOrWhiteSpace(selectItem.Description);

				if (descSpecified)
				{
					stringBuilder.Append($" : {selectItem.Description}");
				}

				stringBuilder.AppendLine();
			}

			stringBuilder.Append(DslKeywords.SelectEnd);

			return stringBuilder.ToString();
		}
	}
}