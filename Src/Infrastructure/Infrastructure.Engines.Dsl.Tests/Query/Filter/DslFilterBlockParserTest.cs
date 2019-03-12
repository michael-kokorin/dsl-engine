namespace Infrastructure.Engines.Dsl.Tests.Query.Filter
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query.Filter;
	using Infrastructure.Engines.Dsl.Query.Filter.Specification;

	using Sprache;

	[TestFixture]
	public sealed class DslFilterBlockParserTest
	{
		[TestCase("var")]
		[TestCase("var123")]
		[TestCase("var_123")]
		public void ShouldParseVariableSpecification(string variableName)
		{
			var query = $"#{variableName}#";

			var result = Execute<FilterConstantSpecification>(query);

			result.Value.Should().BeEquivalentTo(query);
		}

		[TestCase("var")]
		[TestCase("var123")]
		public void ShouldParseParameterSpecification(string variableName)
		{
			var query = "{" + variableName + "}";

			var result = Execute<FilterParameterSpecification>(query);

			result.Value.Should().BeEquivalentTo(query);
		}

		private static IFilterSpecification Execute(string query)
		{
			var result = DslFilterBlockParser.Expression.End().Parse(query);

			result.Should().NotBeNull();

			result.Should().BeAssignableTo<IFilterSpecification>();

			return result;
		}

		private static T Execute<T>(string query) where T : class, IFilterSpecification
		{
			var result = Execute(query);

			result.Should().BeOfType<T>();

			return result as T;
		}

		[TestCase("1")]
		public void ShouldParseConstantSpecification(string value)
		{
			var result = Execute<FilterConstantSpecification>(value);

			result.Value.Should().BeEquivalentTo(value);
		}

		[TestCase("")]
		[TestCase("value")]
		[TestCase("Беломор канал")]
		[TestCase(@"~!@#$%^&*()_+:|\<>?")]
		public void ShouldParseTextConstSpecification(string value)
		{
			var query = $"\"{value}\"";

			var result = Execute<FilterConstantSpecification>(query);

			result.Value.ShouldBeEquivalentTo(query);
		}

		[TestCase("#a#", typeof(FilterConstantSpecification))]
		[TestCase("3", typeof(FilterConstantSpecification))]
		public void ShouldParseGroupSpecification(string value, Type innetSpecificationType)
		{
			var query = $"({value})";

			var result = Execute<FilterGroupSpecification>(query);

			result.Specification.Should().BeOfType(innetSpecificationType);
		}

		[TestCase("null")]
		[TestCase("NULL")]
		public void ShouldParseNullSpecification(string query) =>
			Execute<FilterConstantSpecification>(query).Value.ShouldBeEquivalentTo(query.ToLower());

		[TestCase("true", true)]
		[TestCase("TRUE", true)]
		[TestCase("false", false)]
		[TestCase("FALSE", false)]
		public void ShouldParseBooleanSpecification(string query, bool value) =>
			Execute<FilterConstantSpecification>(query).Value.ShouldBeEquivalentTo(value);

		[Test]
		public void ShouldParseArraySpecification()
		{
			const string query = "(1,\"2\")";

			var result = Execute<FilterArraySpecification>(query);

			var specifications = result.Specifications.ToArray();

			specifications.Count().ShouldBeEquivalentTo(2);
			specifications.First().Should().BeOfType<FilterConstantSpecification>();
			specifications.Last().Should().BeOfType<FilterConstantSpecification>();
		}

		[TestCase("*", FilterOperator.Multiple)]
		[TestCase("/", FilterOperator.Divide)]
		[TestCase("+", FilterOperator.Plus)]
		[TestCase("-", FilterOperator.Minus)]
		[TestCase("<", FilterOperator.Less)]
		[TestCase("<=", FilterOperator.LessOrEqual)]
		[TestCase(">", FilterOperator.Greather)]
		[TestCase(">=", FilterOperator.GreatherOrEqual)]
		[TestCase("==", FilterOperator.Equal)]
		[TestCase("ContAins", FilterOperator.Contains)]
		[TestCase("Is", FilterOperator.Is)]
		[TestCase("Not", FilterOperator.Not)]
		public void ShouldParseFilterSpecification(string opStr, FilterOperator op)
		{
			var query = $"#a# {opStr} #b#";

			var result = Execute<FilterSpecification>(query);

			result.Operator.ShouldBeEquivalentTo(op);
		}

		[TestCase("1 + 2 * 3", FilterOperator.Plus)]
		[TestCase("(1 + 2)\n\t\t\t \r\n\t    *\t3", FilterOperator.Multiple)]
		[TestCase("1 + 2 / 3", FilterOperator.Plus)]
		[TestCase("1 + 2 < 3", FilterOperator.Less)]
		[TestCase("1 + 2 >= 3", FilterOperator.GreatherOrEqual)]
		public void ShouldGetTopOperation(string query, FilterOperator op)
		{
			var result = Execute<FilterSpecification>(query);

			result.Operator.ShouldBeEquivalentTo(op);
		}

		[TestCase("True && False || False", FilterCondition.Or)]
		[TestCase("1 == 2 || 3 + 4 < 2", FilterCondition.Or)]
		[TestCase("1 < 2 || 3 > 4", FilterCondition.Or)]
		[TestCase("1 < 2 && 3 > 4", FilterCondition.And)]
		[TestCase("1 Contains 2 || TRUE", FilterCondition.Or)]
		[TestCase("#a# is Null && False", FilterCondition.And)]
		[TestCase("#a# is Null || #b# < 0", FilterCondition.Or)]
		[TestCase("#a# is Null || (#b# < 0)", FilterCondition.Or)]
		[TestCase("#DisplayName# == 3 && (1 == 3 || 2 < 5)", FilterCondition.And)]
		[TestCase("{DisplayName} == 3 && (1 == 3 || 2 < 5)", FilterCondition.And)]
		public void ShouldTranslateFilterCondition(string query, FilterCondition condition)
		{
			var result = Execute<FilterConditionSpecification>(query);

			result.Condition.ShouldBeEquivalentTo(condition);
		}
	}
}