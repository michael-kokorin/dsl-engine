namespace Infrastructure.Engines.Query
{
	using System;
	using System.Text;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class LimitBlockTranslator : IQueryBlockTranslator<DslLimitBlock>
	{
		public string Translate([NotNull] DslLimitBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var stringBuilder = new StringBuilder();

			if (queryBlock.Skip.HasValue)
			{
				stringBuilder.AppendLine(".Skip(" + queryBlock.Skip.Value + ")");
			}

			if (queryBlock.Take.HasValue)
			{
				stringBuilder.AppendLine(".Take(" + queryBlock.Take.Value + ")");
			}

			return stringBuilder.ToString();
		}

		public string ToDsl([NotNull] DslLimitBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var stringBuilder = new StringBuilder();

			if (queryBlock.Skip != null)
			{
				stringBuilder.AppendLine($"{DslKeywords.Skip} {queryBlock.Skip.Value}");
			}

			if (queryBlock.Take != null)
			{
				stringBuilder.AppendLine($"{DslKeywords.Take} {queryBlock.Take.Value}");
			}

			return stringBuilder.ToString();
		}
	}
}