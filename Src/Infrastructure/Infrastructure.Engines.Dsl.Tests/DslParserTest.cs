namespace Infrastructure.Engines.Dsl.Tests
{
	using System.Data.SqlClient;
	using System.Diagnostics.CodeAnalysis;
	using System.Linq;

	using FluentAssertions;

	using NUnit.Framework;

	using Infrastructure.Engines.Dsl.Query;

	using Sprache;

	[TestFixture]
	[SuppressMessage("ReSharper", "PossibleNullReferenceException")]
	public sealed class DslParserTest
	{
		[SetUp]
		public void Prepare() => _parser = new DslParser();

		private DslParser _parser;

		[Test]
		public void AllSubjectsExpr()
		{
			var result = DslParser.SubjectsExprParse().Parse("subjects all");

			result.Should().NotBeNull();
			result.Roles.Should().BeNullOrEmpty();
			result.SubjectGroups.Should().BeNullOrEmpty();
			result.Persons.Should().BeNullOrEmpty();
			result.IsAll.Should().BeTrue();
		}

		[Test]
		public void FullSubjectsExpr()
		{
			var result = DslParser.SubjectsExprParse().Parse(@"subjects
role alpha; not beta, gamma
role u
group theta; not eta
group f
person max, andr");

			result.Should().NotBeNull();
			result.Roles.Should().HaveCount(2);
			result.Roles[0].RoleName.Should().Be("alpha");
			result.Roles[0].ExcludedPersons.Should().BeEquivalentTo("beta", "gamma");
			result.Roles[1].RoleName.Should().Be("u");
			result.Roles[1].ExcludedPersons.Should().BeEmpty();
			result.SubjectGroups.Should().HaveCount(2);
			result.SubjectGroups[0].GroupName.Should().Be("theta");
			result.SubjectGroups[0].ExcludedPersons.Should().BeEquivalentTo("eta");
			result.SubjectGroups[1].GroupName.Should().Be("f");
			result.SubjectGroups[1].ExcludedPersons.Should().BeEmpty();
			result.Persons.Should().BeEquivalentTo("max", "andr");
		}

		[Test]
		public void OrderExprItemParse_Ascending()
		{
			var result = DslParser.OrderExprItemParse().Parse("#alpha# asc");

			result.Should().NotBeNull();
			result.OrderFieldName.Should().Be("alpha");
			result.SortOrder.Should().Be(SortOrder.Ascending);
		}

		[Test]
		public void OrderExprItemParse_Descending()
		{
			var result = DslParser.OrderExprItemParse().Parse("#alpha# desc");

			result.Should().NotBeNull();
			result.OrderFieldName.Should().Be("alpha");
			result.SortOrder.Should().Be(SortOrder.Descending);
		}

		[Test]
		public void OrderExprParse()
		{
			var result = DslParser.OrderExprParse().Parse(@"order #alpha# asc, #beta# desc
select
select end");

			result.Should().NotBeNull();
			result.Items.Should().HaveCount(2);
			result.Items[0].OrderFieldName.Should().Be("alpha");
			result.Items[0].SortOrder.Should().Be(SortOrder.Ascending);

			result.Items[1].OrderFieldName.Should().Be("beta");
			result.Items[1].SortOrder.Should().Be(SortOrder.Descending);
		}

		[Test]
		public void SubjectsExpr_NotPersons()
		{
			var result = DslParser.SubjectsExprParse().Parse(@"subjects
role alpha; not beta, gamma
role u
group theta; not eta
group f");

			result.Should().NotBeNull();
			result.Roles.Should().HaveCount(2);
			result.Roles[0].RoleName.Should().Be("alpha");
			result.Roles[0].ExcludedPersons.Should().BeEquivalentTo("beta", "gamma");
			result.Roles[1].RoleName.Should().Be("u");
			result.Roles[1].ExcludedPersons.Should().BeEmpty();
			result.SubjectGroups.Should().HaveCount(2);
			result.SubjectGroups[0].GroupName.Should().Be("theta");
			result.SubjectGroups[0].ExcludedPersons.Should().BeEquivalentTo("eta");
			result.SubjectGroups[1].GroupName.Should().Be("f");
			result.SubjectGroups[1].ExcludedPersons.Should().BeEmpty();
			result.Persons.Should().BeNullOrEmpty();
		}

		[Test]
		public void FullSubjectsExpr_NotGroups()
		{
			var result = DslParser.SubjectsExprParse().Parse(@"subjects
role alpha; not beta, gamma
role u
person max, andr");

			result.Should().NotBeNull();
			result.Roles.Should().HaveCount(2);
			result.Roles[0].RoleName.Should().Be("alpha");
			result.Roles[0].ExcludedPersons.Should().BeEquivalentTo("beta", "gamma");
			result.Roles[1].RoleName.Should().Be("u");
			result.Roles[1].ExcludedPersons.Should().BeEmpty();
			result.SubjectGroups.Should().BeNullOrEmpty();
			result.Persons.Should().BeEquivalentTo("max", "andr");
		}

		[Test]
		public void FullSubjectsExpr_NotRoles()
		{
			var result = DslParser.SubjectsExprParse().Parse(@"subjects
group theta; not eta
group f
person max, andr");

			result.Should().NotBeNull();
			result.Roles.Should().BeNullOrEmpty();
			result.SubjectGroups.Should().HaveCount(2);
			result.SubjectGroups[0].GroupName.Should().Be("theta");
			result.SubjectGroups[0].ExcludedPersons.Should().BeEquivalentTo("eta");
			result.SubjectGroups[1].GroupName.Should().Be("f");
			result.SubjectGroups[1].ExcludedPersons.Should().BeEmpty();
			result.Persons.Should().BeEquivalentTo("max", "andr");
		}

		[Test]
		public void FullSubjectsExpr_NotRolesGroups()
		{
			var result = DslParser.SubjectsExprParse().Parse(@"subjects
person max, andr");

			result.Should().NotBeNull();
			result.Roles.Should().BeNullOrEmpty();
			result.SubjectGroups.Should().BeNullOrEmpty();
			result.Persons.Should().BeEquivalentTo("max", "andr");
		}

		[Test]
		public void AnyEventExpr()
		{
			var result = DslParser.EventExprParse().Parse("event any");

			result.Should().NotBeNull();
			result.Should().BeOfType<GroupExpr.AnyGroupExpr>();

			result.Dependent.Should().NotBeNull();

			// ReSharper disable once RedundantExplicitParamsArrayCreation
			result.Dependent.Should().BeEquivalentTo(new string[0]);
		}

		[Test]
		public void BooleanParseFalse()
		{
			var result = DslParser.BooleanParse().Parse("false");

			result.Should().BeFalse();
		}

		[Test]
		public void BooleanParseTrue()
		{
			var result = DslParser.BooleanParse().Parse("true");

			result.Should().BeTrue();
		}

		[Test]
		public void DateParse()
		{
			var result = DslParser.DateParse().Parse("2015-08-09");

			result.Year.Should().Be(2015);
			result.Month.Should().Be(8);
			result.Day.Should().Be(9);
		}

		[Test]
		public void DateTimeParse()
		{
			var result = DslParser.DateTimeParse().Parse("2015-10-11 04:05:06");

			result.Year.Should().Be(2015);
			result.Month.Should().Be(10);
			result.Day.Should().Be(11);

			result.Hour.Should().Be(4);
			result.Minute.Should().Be(5);
			result.Second.Should().Be(6);
		}

		[Test]
		public void DateTimeParse_WithEmptySeconds()
		{
			var result = DslParser.DateTimeParse().Parse("2015-10-11 04:05");

			result.Year.Should().Be(2015);
			result.Month.Should().Be(10);
			result.Day.Should().Be(11);

			result.Hour.Should().Be(4);
			result.Minute.Should().Be(5);
			result.Second.Should().Be(0);
		}

		[Test]
		public void DeliveryProtocolExpr()
		{
			const string protocol = "email";
			var result = DslParser.DeliveryProtocolExprParse().Parse("protocol " + protocol);

			result.Should().NotBeNull();
			result.Value.ShouldBeEquivalentTo(protocol);
		}

		[Test]
		public void ExcludeEventExpr()
		{
			var result = DslParser.EventExprParse().Parse("event ! x,y");

			result.Should().NotBeNull().And.BeOfType<GroupExpr.ExcludeGroupExpr>();
			result.Dependent.Should().NotBeNull();
			result.Dependent.Should().BeEquivalentTo("x", "y");
		}

		[Test]
		public void IncludeEventExpr()
		{
			var result = DslParser.EventExprParse().Parse("event x,y");

			result.Should().NotBeNull();
			result.Should().BeOfType<GroupExpr.IncludeGroupExpr>();
			result.Dependent.Should().NotBeNull();
			result.Dependent.Should().BeEquivalentTo("x", "y");
		}

		[Test]
		public void EventNotificationRuleExpr()
		{
			var result = _parser.NotificationRuleExprParse(
				@"event x,y
								subjects
role alpha; not beta, gamma
role u
group theta; not eta
group f
person max, andr
								protocol email
								report 131231231232
attach
report 75
format snmp");

			result.Should().NotBeNull();
			result.Event.Should().NotBeNull();
			result.Event.Should().BeOfType<GroupExpr.IncludeGroupExpr>();
			result.Event.Dependent.Should().NotBeNull();
			result.Event.Dependent.Should().BeEquivalentTo("x", "y");

			result.Trigger.Should().BeNull();

			result.Subjects.Should().NotBeNull();
			result.Subjects.Roles.Should().HaveCount(2);
			result.Subjects.Roles[0].RoleName.Should().Be("alpha");
			result.Subjects.Roles[0].ExcludedPersons.Should().BeEquivalentTo("beta", "gamma");
			result.Subjects.Roles[1].RoleName.Should().Be("u");
			result.Subjects.Roles[1].ExcludedPersons.Should().BeEmpty();
			result.Subjects.SubjectGroups.Should().HaveCount(2);
			result.Subjects.SubjectGroups[0].GroupName.Should().Be("theta");
			result.Subjects.SubjectGroups[0].ExcludedPersons.Should().BeEquivalentTo("eta");
			result.Subjects.SubjectGroups[1].GroupName.Should().Be("f");
			result.Subjects.SubjectGroups[1].ExcludedPersons.Should().BeEmpty();
			result.Subjects.Persons.Should().BeEquivalentTo("max", "andr");

			result.Protocol.Should().NotBeNull();
			result.Protocol.ShouldBeEquivalentTo("email");

			result.ReportId.ShouldBeEquivalentTo(131231231232);

			result.Attachments.Should().HaveCount(1);

			var attach = result.Attachments.First();

			attach.ReportId.ShouldBeEquivalentTo(75);
			attach.ExportFormat.ShouldBeEquivalentTo("snmp");
			attach.Parameters.Should().HaveCount(0);
		}

		[Test]
		public void TimeNotificationRuleExpr()
		{
			var result = _parser.NotificationRuleExprParse(
				@"trigger 2015-08-09 01:02:03 repeat 4d5h6m7s
								subjects
role alpha; not beta, gamma
role u
group theta; not eta
group f
person max, andr
								protocol email
report 12310234
attach
report 75
format snmp
parameters
a: b;
");

			result.Should().NotBeNull();
			result.Event.Should().BeNull();

			result.Trigger.Should().NotBeNull();
			result.Trigger.Start.Year.Should().Be(2015);
			result.Trigger.Start.Month.Should().Be(8);
			result.Trigger.Start.Day.Should().Be(9);

			result.Trigger.Start.Hour.Should().Be(1);
			result.Trigger.Start.Minute.Should().Be(2);
			result.Trigger.Start.Second.Should().Be(3);

			result.Trigger.Repeat.HasValue.Should().BeTrue();
			result.Trigger.Repeat?.Days.Should().Be(4);
			result.Trigger.Repeat?.Hours.Should().Be(5);
			result.Trigger.Repeat?.Minutes.Should().Be(6);
			result.Trigger.Repeat?.Seconds.Should().Be(7);

			result.Subjects.Should().NotBeNull();
			result.Subjects.Roles.Should().HaveCount(2);
			result.Subjects.Roles[0].RoleName.Should().Be("alpha");
			result.Subjects.Roles[0].ExcludedPersons.Should().BeEquivalentTo("beta", "gamma");
			result.Subjects.Roles[1].RoleName.Should().Be("u");
			result.Subjects.Roles[1].ExcludedPersons.Should().BeEmpty();
			result.Subjects.SubjectGroups.Should().HaveCount(2);
			result.Subjects.SubjectGroups[0].GroupName.Should().Be("theta");
			result.Subjects.SubjectGroups[0].ExcludedPersons.Should().BeEquivalentTo("eta");
			result.Subjects.SubjectGroups[1].GroupName.Should().Be("f");
			result.Subjects.SubjectGroups[1].ExcludedPersons.Should().BeEmpty();
			result.Subjects.Persons.Should().BeEquivalentTo("max", "andr");

			result.Protocol.Should().NotBeNull();
			result.Protocol.ShouldBeEquivalentTo("email");

			result.ReportId.ShouldBeEquivalentTo(12310234);

			result.Attachments.Should().HaveCount(1);

			var attach = result.Attachments.First();

			attach.ReportId.ShouldBeEquivalentTo(75);
			attach.ExportFormat.ShouldBeEquivalentTo("snmp");
			attach.Parameters.Should().HaveCount(1);

			var attachParam = attach.Parameters.First();

			attachParam.Key.ShouldBeEquivalentTo("a");
			attachParam.Value.ShouldBeEquivalentTo("b");
		}

		[Test]
		public void NotificationReportExpr()
		{
			const long reportId = 21499234;
			var result = DslParser.NotificationReportIdParse().Parse("report " + reportId);

			result.Should().NotBeNull();
			result.Value.ShouldBeEquivalentTo(reportId);
		}

		[Test]
		public void AttachExprParse()
		{
			const string expr = @"report 123
format snmp
parameters
a: b;";

			var result = DslParser.ParseReportAttachmentBlockExpr().Parse(expr);

			result.ReportId.ShouldBeEquivalentTo(123);
			result.ExportFormat.ShouldBeEquivalentTo("snmp");
			result.Parameters.Should().HaveCount(1);

			var param = result.Parameters.First();

			param.Key.ShouldBeEquivalentTo("a");
			param.Value.ShouldBeEquivalentTo("b");
		}

		[Test]
		public void TimeParse()
		{
			var result = DslParser.TimeParse().Parse("01:02:03");

			result.Hours.Should().Be(1);
			result.Minutes.Should().Be(2);
			result.Seconds.Should().Be(3);
		}

		[Test]
		public void TimeParse_WithEmptySeconds()
		{
			var result = DslParser.TimeParse().Parse("01:02");

			result.Hours.Should().Be(1);
			result.Minutes.Should().Be(2);
			result.Seconds.Should().Be(0);
		}

		[Test]
		public void TimeShiftParse()
		{
			var result = DslParser.TimeShiftParse().Parse("1d2h3m4s");

			result.Days.Should().Be(1);
			result.Hours.Should().Be(2);
			result.Minutes.Should().Be(3);
			result.Seconds.Should().Be(4);
		}

		[Test]
		public void TimeTriggerExprParse()
		{
			var result = DslParser.TimeTriggerExprParse().Parse("trigger 2015-08-09 01:02:03 repeat 4d5h6m7s");

			result.Start.Year.Should().Be(2015);
			result.Start.Month.Should().Be(8);
			result.Start.Day.Should().Be(9);

			result.Start.Hour.Should().Be(1);
			result.Start.Minute.Should().Be(2);
			result.Start.Second.Should().Be(3);

			result.Repeat.HasValue.Should().BeTrue();
			result.Repeat?.Days.Should().Be(4);
			result.Repeat?.Hours.Should().Be(5);
			result.Repeat?.Minutes.Should().Be(6);
			result.Repeat?.Seconds.Should().Be(7);
		}

		[Test]
		public void TimeTriggerExprParse_WithoutRepeat()
		{
			var result = DslParser.TimeTriggerExprParse().Parse("trigger 2015-08-09 01:02:03");

			result.Start.Year.Should().Be(2015);
			result.Start.Month.Should().Be(8);
			result.Start.Day.Should().Be(9);

			result.Start.Hour.Should().Be(1);
			result.Start.Minute.Should().Be(2);
			result.Start.Second.Should().Be(3);

			result.Repeat.HasValue.Should().BeFalse();
		}

		[Test]
		public void FirstQueryModeParse_First()
		{
			var result = DslParser.FirstQueryModeParse().Parse("first");

			result.Should().Be(FirstQueryMode.First);
		}

		[Test]
		public void FirstQueryModeParse_FirstOrDefault()
		{
			var result = DslParser.FirstQueryModeParse().Parse("firstordefault");

			result.Should().Be(FirstQueryMode.FirstOrDefault);
		}

		[Test]
		public void SelectItemParse_NameValue()
		{
			var result = DslParser.SelectItemExprParse().Parse("alpha = beta");

			result.Should().NotBeNull();
			result.Name.Should().Be("alpha");
			result.Value.Should().Be("beta");
		}

		[Test]
		public void SelectItemParse_NameValueDisplayName()
		{
			var result = DslParser.SelectItemExprParse().Parse("alpha = beta@Поле");

			result.Should().NotBeNull();
			result.Name.Should().Be("alpha");
			result.Value.Should().Be("beta");
			result.DisplayName.Should().Be("Поле");
		}

		[Test]
		public void SelectItemParse_Value()
		{
			var result = DslParser.SelectItemExprParse().Parse("beta");

			result.Should().NotBeNull();
			result.Name.Should().BeNull();
			result.Value.Should().Be("beta");
		}

		[Test]
		public void SelectItemParse_NameValueWithPoints()
		{
			var result = DslParser.SelectItemExprParse().Parse("alpha = beta.gamma.theta");

			result.Should().NotBeNull();
			result.Name.Should().Be("alpha");
			result.Value.Should().Be("beta.gamma.theta");
		}

		[Test]
		public void SelectItemParse_ValueWithPoints()
		{
			var result = DslParser.SelectItemExprParse().Parse("beta.gamma.theta");

			result.Should().NotBeNull();
			result.Name.Should().BeNull();
			result.Value.Should().Be("beta.gamma.theta");
		}

		[Test]
		public void SelectParse()
		{
			var result = DslParser.SelectExprParse().Parse(@"select
alpha = #beta#
#gamma#@my own
theta = #eta.sigma.pi#
#x.y.z#
select end");

			result.Should().NotBeNull();

			var dslFormatBlock = result as DslFormatBlock;

			result.Should().NotBeNull();

			dslFormatBlock.Selects.Should().NotBeNullOrEmpty();
			dslFormatBlock.Selects.Should().HaveCount(4);

			var first = dslFormatBlock.Selects.First();
			first.Name.Should().Be("alpha");
			first.Value.Should().Be("#beta#");

			var second = dslFormatBlock.Selects.Skip(1).First();
			second.Name.Should().BeNull();
			second.Value.Should().Be("#gamma#");
			second.DisplayName.Should().Be("my own");

			var third = dslFormatBlock.Selects.Skip(2).First();
			third.Name.Should().Be("theta");
			third.Value.Should().Be("#eta.sigma.pi#");

			var fourth = dslFormatBlock.Selects.Skip(3).First();
			fourth.Name.Should().BeNull();
			fourth.Value.Should().Be("#x.y.z#");
		}

		[Test]
		public void PolicyRuleExprParse()
		{
			var result = _parser.PolicyRuleExprParse(@"Entity = query Tasks
where #Id# == {TaskId}
select
High = #HighSeverityVulns#
select end
first;");

			result.Data.Should().NotBeNullOrEmpty();
		}

		[Test]
		public void QueryExprParse()
		{
			var result = _parser.DataQueryParse(@"Tasks
order #a# asc,#b# desc,#c# asc
group #aa#,#bb#,#cc#
select
alpha = #beta#
#gamma#
theta = #eta.sigma.pi#
#x.y.z#
select end
skip 1
take 2
table Id");

			result.Should().NotBeNull();

			result.Blocks.Should().NotBeNull();

			var order = result.Blocks.First() as DslOrderBlock;

			order.Should().NotBeNull();

			order.Should().NotBeNull();
			order.Items.Should().HaveCount(3);
			order.Items[0].OrderFieldName.Should().Be("a");
			order.Items[0].SortOrder.Should().Be(SortOrder.Ascending);

			order.Items[1].OrderFieldName.Should().Be("b");
			order.Items[1].SortOrder.Should().Be(SortOrder.Descending);

			order.Items[2].OrderFieldName.Should().Be("c");
			order.Items[2].SortOrder.Should().Be(SortOrder.Ascending);

			var groupBlock = result.Blocks.Skip(1).First() as DslGroupBlock;

			groupBlock.Should().NotBeNull();

			groupBlock.Items.Should().NotBeNullOrEmpty();
			groupBlock.Items.Select(_ => _.VariableName).Should().BeEquivalentTo("aa", "bb", "cc");

			var select = result.Blocks.Skip(2).First() as DslFormatBlock;

			select.Should().NotBeNull();

			select.Should().NotBeNull();
			select.Selects.Should().NotBeNullOrEmpty();
			select.Selects.Should().HaveCount(4);
			var first = select.Selects.First();
			first.Name.Should().Be("alpha");
			first.Value.Should().Be("#beta#");

			var second = select.Selects.Skip(1).First();
			second.Name.Should().BeNull();
			second.Value.Should().Be("#gamma#");

			var third = select.Selects.Skip(2).First();
			third.Name.Should().Be("theta");
			third.Value.Should().Be("#eta.sigma.pi#");

			var fourth = select.Selects.Skip(3).First();
			fourth.Name.Should().BeNull();
			fourth.Value.Should().Be("#x.y.z#");

			var skipLimit = result.Blocks.Skip(3).First() as DslLimitBlock;

			skipLimit.Should().NotBeNull();

			skipLimit.Skip.ShouldBeEquivalentTo(1);

			var takeLimit = result.Blocks.Skip(4).First() as DslLimitBlock;

			takeLimit.Should().NotBeNull();

			takeLimit.Take.ShouldBeEquivalentTo(2);

			result.TableKey.Should().Be("Id");
		}
	}
}