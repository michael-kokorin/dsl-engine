namespace Infrastructure.Engines.Dsl.Tests
{
    using System;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public sealed class GroupExprTest
    {
        [Test]
        public static void AnyGroupExpr()
        {
            var expr = GroupExpr.Any;

            expr.Should().NotBeNull();
            expr.IsMatch(string.Empty).Should().BeTrue();
            ((Action) (() => expr.IsMatch(null))).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public static void ExcludeGroupExpr()
        {
            var expr = GroupExpr.Exclude("one", "two");

            expr.Should().NotBeNull();
            expr.IsMatch("one").Should().BeFalse();
            expr.IsMatch("two").Should().BeFalse();
            expr.IsMatch("three").Should().BeTrue();
            expr.IsMatch(string.Empty).Should().BeFalse();
            ((Action) (() => expr.IsMatch(null))).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public static void ExcludeGroupExpr_EmptyList()
        {
            Action action = () => GroupExpr.Exclude();

            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public static void IncludeGroupExpr()
        {
            var expr = GroupExpr.Include("one", "two");

            expr.Should().NotBeNull();
            expr.IsMatch("one").Should().BeTrue();
            expr.IsMatch("two").Should().BeTrue();
            expr.IsMatch("three").Should().BeFalse();
            expr.IsMatch(string.Empty).Should().BeFalse();
            ((Action) (() => expr.IsMatch(null))).ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public static void IncludeGroupExpr_EmptyList()
        {
            Action action = () => GroupExpr.Include();

            action.ShouldThrow<ArgumentNullException>();
        }
    }
}