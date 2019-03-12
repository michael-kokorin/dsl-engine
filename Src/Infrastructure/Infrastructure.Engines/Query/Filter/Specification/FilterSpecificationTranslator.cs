namespace Infrastructure.Engines.Query.Filter.Specification
{
	using System;
	using System.ComponentModel;

	using JetBrains.Annotations;

	using Infrastructure.Engines.Dsl.Query.Filter;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	[UsedImplicitly]
	internal sealed class FilterSpecificationTranslator : IFilterSpecificationTranslator<FilterSpecification>
	{
		private readonly IFilterSpecificationTranslatorDirector _filterSpecificationTranslatorDirector;

		public FilterSpecificationTranslator(
			[NotNull] IFilterSpecificationTranslatorDirector filterSpecificationTranslatorDirector)
		{
			if (filterSpecificationTranslatorDirector == null)
				throw new ArgumentNullException(nameof(filterSpecificationTranslatorDirector));

			_filterSpecificationTranslatorDirector = filterSpecificationTranslatorDirector;
		}

		public string Translate([NotNull] FilterSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var leftTranslated = _filterSpecificationTranslatorDirector.Translate((dynamic) specification.LeftSpecification);

			var rightTranslated = _filterSpecificationTranslatorDirector.Translate((dynamic) specification.RightSpecification);

			switch (specification.Operator)
			{
				case FilterOperator.In:
					return $"{rightTranslated}.Contains({leftTranslated})";
				case FilterOperator.Contains:
					return $"{leftTranslated}.Contains({rightTranslated})";
			}

			var operation = Translate(specification.Operator);

			return $"{leftTranslated} {operation} {rightTranslated}";
		}

		public string ToDsl([NotNull] FilterSpecification specification)
		{
			if (specification == null) throw new ArgumentNullException(nameof(specification));

			var leftDsl = _filterSpecificationTranslatorDirector.ToDsl((dynamic) specification.LeftSpecification);

			var rightDsl = _filterSpecificationTranslatorDirector.ToDsl((dynamic) specification.RightSpecification);

			var op = ToDsl(specification.Operator);

			return $"{leftDsl} {op} {rightDsl}";
		}

		private string Translate(FilterOperator op)
		{
			switch (op)
			{
				case FilterOperator.Divide:
					return "/";
				case FilterOperator.Equal:
				case FilterOperator.Is:
					return "==";
				case FilterOperator.Greather:
					return ">";
				case FilterOperator.GreatherOrEqual:
					return ">=";
				case FilterOperator.Less:
					return "<";
				case FilterOperator.LessOrEqual:
					return "<=";
				case FilterOperator.Minus:
					return "-";
				case FilterOperator.Multiple:
					return "*";
				case FilterOperator.Not:
					return "!=";
				case FilterOperator.Plus:
					return "+";
				default:
					throw new InvalidEnumArgumentException(nameof(op));
			}
		}

		private string ToDsl(FilterOperator op)
		{
			switch (op)
			{
				case FilterOperator.Contains:
					return "CONTAINS";
				case FilterOperator.Divide:
					return "/";
				case FilterOperator.Equal:
					return "==";
				case FilterOperator.Greather:
					return ">";
				case FilterOperator.GreatherOrEqual:
					return ">=";
				case FilterOperator.In:
					return "IN";
				case FilterOperator.Is:
					return "IS";
				case FilterOperator.Less:
					return "<";
				case FilterOperator.LessOrEqual:
					return "<=";
				case FilterOperator.Minus:
					return "-";
				case FilterOperator.Multiple:
					return "*";
				case FilterOperator.Not:
					return "!=";
				case FilterOperator.Plus:
					return "+";
				default:
					throw new InvalidEnumArgumentException(nameof(op));
			}
		}
	}
}