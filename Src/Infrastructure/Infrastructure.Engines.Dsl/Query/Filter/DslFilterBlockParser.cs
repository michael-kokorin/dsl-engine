namespace Infrastructure.Engines.Dsl.Query.Filter
{
	using System;

	using Common.Extensions;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	using Sprache;

	internal static class DslFilterBlockParser
	{
		public static IDslQueryBlock ParseQuery(string query) => ParseFilterBlock().Parse(query);

		private static Parser<IDslQueryBlock> ParseFilterBlock() =>
			from specification in Expression.End()
			select new DslFilterBlock
			{
				Specification = specification
			};

		private static readonly Parser<IFilterSpecification> Variable =
			from quote in ToKeyword(FilterKeywords.VariableOpenTag)
			from identifier in Parse.LetterOrDigit
				.Or(Parse.CharExcept(FilterKeywords.VariableOpenTag.GetDescription()))
				.AtLeastOnce()
				.Text()
			from quoteEnd in ToKeyword(FilterKeywords.VariableCloseTag)
			select new FilterConstantSpecification
			{
				Value = $"{FilterKeywords.VariableOpenTag.GetDescription()}{identifier}{FilterKeywords.VariableCloseTag.GetDescription()}"
			};

		private static readonly Parser<IFilterSpecification> Parameter =
			from quote in ToKeyword(FilterKeywords.ParameterOpenTag)
			from identifier in Parse.LetterOrDigit
				.AtLeastOnce()
				.Text()
			from quoteEnd in ToKeyword(FilterKeywords.ParameterCloseTag)
			select new FilterParameterSpecification
			{
				Value =
					$"{FilterKeywords.ParameterOpenTag.GetDescription()}{identifier}{FilterKeywords.ParameterCloseTag.GetDescription()}"
			};

		private static readonly Parser<IFilterSpecification> Constant =
			from value in Parse.Digit.Many().Text()
			select new FilterConstantSpecification
			{
				Value = value
			};

		private static readonly Parser<IFilterSpecification> Null =
			from value in ToKeyword(FilterKeywords.Null)
			select new FilterConstantSpecification
			{
				Value = value.ToLower()
			};

		private static readonly Parser<IFilterSpecification> Boolean =
			from value in ToKeyword(FilterKeywords.True).Or(ToKeyword(FilterKeywords.False))
			select new FilterConstantSpecification
			{
				Value = Convert.ToBoolean(value).ToString().ToLower()
			};

		private static readonly Parser<IFilterSpecification> TextConstant =
			from quote in ToKeyword(FilterKeywords.TextConstantOpenTag)
			from identifier in Parse.CharExcept(FilterKeywords.TextConstantOpenTag.GetDescription())
				.Many()
				.Text()
				.Optional()
			from quoteEnd in ToKeyword(FilterKeywords.TextConstantCloseTag)
			select new FilterConstantSpecification
			{
				Value = $"{FilterKeywords.TextConstantOpenTag.GetDescription()}{identifier.GetOrDefault()}{FilterKeywords.TextConstantCloseTag.GetDescription()}"
			};

		private static readonly Parser<FilterCondition> And =
			MakeCondition(FilterCondition.And);

		private static readonly Parser<FilterOperator> Contain =
			MakeOperator(FilterOperator.Contains);

		private static readonly Parser<FilterOperator> Divide =
			MakeOperator(FilterOperator.Divide);

		private static readonly Parser<FilterOperator> Eq =
			MakeOperator(FilterOperator.Equal);

		private static readonly Parser<FilterOperator> Greather =
			MakeOperator(FilterOperator.Greather);

		private static readonly Parser<FilterOperator> GreatherOrEqual =
			MakeOperator(FilterOperator.GreatherOrEqual);

		private static readonly Parser<FilterOperator> Is =
			MakeOperator(FilterOperator.Is);

		private static readonly Parser<FilterOperator> In =
			MakeOperator(FilterOperator.In);

		private static readonly Parser<FilterOperator> Less =
			MakeOperator(FilterOperator.Less);

		private static readonly Parser<FilterOperator> LLessOrEqualess =
			MakeOperator(FilterOperator.LessOrEqual);

		private static readonly Parser<FilterOperator> Minus =
			MakeOperator(FilterOperator.Minus);

		private static readonly Parser<FilterOperator> Multiple =
			MakeOperator(FilterOperator.Multiple);

		private static readonly Parser<FilterOperator> Not =
			MakeOperator(FilterOperator.Not);

		private static readonly Parser<FilterCondition> Or =
			MakeCondition(FilterCondition.Or);

		private static readonly Parser<FilterOperator> Plus =
			MakeOperator(FilterOperator.Plus);

		private static readonly Parser<IFilterSpecification> Array =
			Parse
				.Ref(() => Expression)
				.DelimitedBy(ToKeyword(FilterKeywords.ArrayDelimiter))
				.Contained(
					ToKeyword(FilterKeywords.ArrayOpenTag),
					ToKeyword(FilterKeywords.ArrayCloseTag))
				.Select(_ => new FilterArraySpecification
				{
					Specifications = _
				});

		private static readonly Parser<IFilterSpecification> Group =
			Parse
				.Ref(() => Expression)
				.Contained(
					ToKeyword(FilterKeywords.GroupOpenTag),
					ToKeyword(FilterKeywords.GroupCloseTag))
				.Select(_ => new FilterGroupSpecification
				{
					Specification = _
				});


		private static readonly Parser<IFilterSpecification> Unions =
			Group
				.Or(Array)
				.Token();

		private static readonly Parser<IFilterSpecification> Primitives =
			Unions
				.Or(TextConstant)
				.Or(Variable)
				.Or(Parameter)
				.Or(Boolean)
				.Or(Null)
				.Or(Constant)
				.Token();

		private static readonly Parser<IFilterSpecification> Multiplition =
			Parse.ChainOperator(Multiple.Or(Divide), Primitives, GetBinaryOperation);

		private static readonly Parser<IFilterSpecification> Addition =
			Parse.ChainOperator(Plus.Or(Minus), Multiplition, GetBinaryOperation);

		private static readonly Parser<IFilterSpecification> SoftEquality =
			Parse.ChainOperator(LLessOrEqualess.Or(GreatherOrEqual),
				Addition,
				GetBinaryOperation);

		private static readonly Parser<IFilterSpecification> Equality =
			Parse.ChainOperator(Eq.Or(Less).Or(Greather).Or(Contain).Or(Is).Or(Not).Or(In),
				SoftEquality,
				GetBinaryOperation);

		private static readonly Parser<IFilterSpecification> ConditionalAnd =
			Parse.ChainOperator(And, Equality, GetCondition);

		private static readonly Parser<IFilterSpecification> ConditionalOr =
			Parse.ChainOperator(Or, ConditionalAnd, GetCondition);

		internal static readonly Parser<IFilterSpecification> Expression =
			ConditionalOr;

		private static Parser<string> ToKeyword(FilterKeywords keyword)
		{
			var keywordRegexp = keyword.GetDescription();

			return Parse.Regex(keywordRegexp).Token();
		}

		private static Parser<FilterCondition> MakeCondition(FilterCondition condition) =>
			MakeCondition(condition.GetDescription(), condition);

		private static Parser<FilterCondition> MakeCondition(string conditionName, FilterCondition condition) =>
			Parse.Regex(conditionName).Return(condition).Token();

		private static IFilterSpecification GetCondition(FilterCondition condition,
			IFilterSpecification left,
			IFilterSpecification right) =>
				new FilterConditionSpecification
				{
					Condition = condition,
					LeftSpecification = left,
					RightSpecification = right
				};

		private static Parser<FilterOperator> MakeOperator(FilterOperator op) =>
			MakeOperator(op.GetDescription(), op);

		private static Parser<FilterOperator> MakeOperator(string operatorName, FilterOperator op) =>
			Parse.Regex(operatorName).Return(op).Token();

		private static IFilterSpecification GetBinaryOperation(FilterOperator op,
			IFilterSpecification left,
			IFilterSpecification right) =>
				new FilterSpecification
				{
					LeftSpecification = left,
					Operator = op,
					RightSpecification = right
				};
	}
}