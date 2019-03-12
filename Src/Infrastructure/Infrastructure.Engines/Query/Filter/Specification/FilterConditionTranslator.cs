namespace Infrastructure.Engines.Query.Filter.Specification
{
	using System;
	using System.ComponentModel;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query.Filter;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	public sealed class FilterConditionTranslator : IFilterSpecificationTranslator<FilterConditionSpecification>
	{
		private readonly IFilterSpecificationTranslatorDirector _filterSpecificationTranslatorDirector;

		public FilterConditionTranslator(
			[NotNull] IFilterSpecificationTranslatorDirector filterSpecificationTranslatorDirector)
		{
			if (filterSpecificationTranslatorDirector == null)
				throw new ArgumentNullException(nameof(filterSpecificationTranslatorDirector));

			_filterSpecificationTranslatorDirector = filterSpecificationTranslatorDirector;
		}

		public string Translate(FilterConditionSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var leftTranslated = _filterSpecificationTranslatorDirector.Translate((dynamic)specification.LeftSpecification);

			var rightTranslated = _filterSpecificationTranslatorDirector.Translate((dynamic)specification.RightSpecification);

			var operation = Translate(specification.Condition);

			return $"{leftTranslated} {operation} {rightTranslated}";
		}

		private string Translate(FilterCondition condition)
		{
			switch (condition)
			{
				case FilterCondition.And:
					return "&&";
				case FilterCondition.Or:
					return "||";
				default:
					throw new InvalidEnumArgumentException(nameof(condition));
			}
		}

		public string ToDsl(FilterConditionSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var leftDsl = _filterSpecificationTranslatorDirector.ToDsl((dynamic)specification.LeftSpecification);

			var rightDsl = _filterSpecificationTranslatorDirector.ToDsl((dynamic)specification.RightSpecification);

			var op = ToDsl(specification.Condition);

			return $"{leftDsl} {op} {rightDsl}";
		}

		private string ToDsl(FilterCondition op)
		{
			switch (op)
			{
				case FilterCondition.And:
					return "&&";
				case FilterCondition.Or:
					return "||";
				default:
					throw new InvalidEnumArgumentException(nameof(op));
			}
		}
	}
}
