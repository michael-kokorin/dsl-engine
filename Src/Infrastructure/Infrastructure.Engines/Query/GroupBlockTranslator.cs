namespace Infrastructure.Engines.Query
{
	using System;
	using System.Linq;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl;
	using Infrastructure.Engines.Dsl.Query;

	[UsedImplicitly]
	internal sealed class GroupBlockTranslator : IQueryBlockTranslator<DslGroupBlock>
	{
		private readonly IQueryVariableNameBuilder _queryVariableNameBuilder;

		public GroupBlockTranslator([NotNull] IQueryVariableNameBuilder queryVariableNameBuilder)
		{
			if (queryVariableNameBuilder == null) throw new ArgumentNullException(nameof(queryVariableNameBuilder));

			_queryVariableNameBuilder = queryVariableNameBuilder;
		}

		public string Translate([NotNull] DslGroupBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			var properties = queryBlock.Items.Select(_ =>
			_queryVariableNameBuilder.ToProperty(
				_queryVariableNameBuilder.Encode(_.VariableName)));

			return $".GroupBy(x => new{{{string.Join(",", properties)}}})";
		}

		public string ToDsl([NotNull] DslGroupBlock queryBlock)
		{
			if (queryBlock == null) throw new ArgumentNullException(nameof(queryBlock));

			return $"{DslKeywords.Group} {string.Join(",", queryBlock.Items.Select(_ => _queryVariableNameBuilder.Encode(_.VariableName)))}";
		}
	}
}