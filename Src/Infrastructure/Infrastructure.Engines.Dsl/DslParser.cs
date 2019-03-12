namespace Infrastructure.Engines.Dsl
{
	using System;
	using System.Collections.Generic;
	using System.Data.SqlClient;
	using System.Linq;

	using Infrastructure.Engines.Dsl.Query;
	using Infrastructure.Engines.Dsl.Query.Filter;

	using Sprache;

	internal sealed class DslParser : IDslParser
	{
		public static Parser<bool> BooleanParse() =>
			Parse.String("true").Token().Return(true)
				.Or(Parse.String("false").Token().Return(false).Token());

		public static Parser<DateTime> DateParse() =>
			from year in Parse.Number
			from separator in Parse.Char('-')
			from month in Parse.Number
			from separator2 in Parse.Char('-')
			from day in Parse.Number
			select new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

		public static Parser<TimeSpan> TimeParse() =>
			from hour in Parse.Number
			from separator in Parse.Char(':')
			from minute in Parse.Number
			from second in (
				from separator2 in Parse.Char(':')
				from seconds in Parse.Number
				select seconds).Optional()
			select
				new TimeSpan(int.Parse(hour),
					int.Parse(minute),
					second.IsDefined && !second.IsEmpty ? int.Parse(second.Get()) : 0);

		public static Parser<DateTime> DateTimeParse() =>
			from date in DateParse().Token()
			from time in TimeParse().Optional()
			select date.Add(time.GetOrDefault());

		public static Parser<TimeSpan> TimeShiftParse() =>
			from days in TimeShiftBlockParse("d").Optional()
			from hours in TimeShiftBlockParse("h").Optional()
			from minutes in TimeShiftBlockParse("m").Optional()
			from seconds in TimeShiftBlockParse("s").Optional()
			select
				new TimeSpan(days.GetOrDefault(), hours.GetOrDefault(), minutes.GetOrDefault(), seconds.GetOrDefault());

		public static Parser<TimeTriggerExpr> TimeTriggerExprParse() =>
			from title in Parse.String(DslKeywords.TimeTrigger)
			from start in DateTimeParse().Token()
			from repeat in (
				from repeatTitle in Parse.String(DslKeywords.RepeatTimeTrigger).Token()
				from time in TimeShiftParse().Token()
				select time).Optional()
			select new TimeTriggerExpr(start, repeat.IsDefined ? repeat.Get() : (TimeSpan?) null);

		private static Parser<GroupExpr> GroupExprParse(string mainTag, string allItemsTag) =>
			(from eventTitle in Parse.String(mainTag).Token()
				from content in Parse.String(allItemsTag).Token()
				select GroupExpr.Any)
				.Or(
					from eventTitle in Parse.String(mainTag).Token()
					from negation in Parse.Char('!').Optional().Token()
					from identifier in IdentifierListParse()
					select negation.IsDefined && !negation.IsEmpty
						? GroupExpr.Exclude(identifier.ToArray())
						: GroupExpr.Include(identifier.ToArray())
				);

		public static Parser<GroupExpr> EventExprParse() => GroupExprParse("event", "any");

		public static Parser<SubjectsExpr> SubjectsExprParse() =>
			from startToken in Parse.String("subjects").Token()
			from allToken in Parse.String("all").Token().Optional()
			from roles in RoleExprParse().Token().Many().Optional()
			from groups in SubjectGroupExprParse().Token().Many().Optional()
			from persons in (
				from personTitle in Parse.String("person")
				from items in IdentifierListParse()
				select items).Token().Optional()
			select new SubjectsExpr
			{
				IsAll = allToken.IsDefined,
				Roles = allToken.IsDefined ? new RoleExpr[0] : roles.IsDefined ? roles.Get().ToArray() : null,
				SubjectGroups = allToken.IsDefined ? new SubjectGroupExpr[0] : groups.IsDefined ? groups.Get().ToArray() : null,
				Persons = allToken.IsDefined ? new string[0] : persons.IsDefined ? persons.Get().ToArray() : null
			};

		private static Parser<RoleExpr> RoleExprParse() =>
			from token in Parse.String("role").Token()
			from roleTitle in Parse.CharExcept(new[]
			{
				';',
				'\n'
			}).AtLeastOnce().Text().Token()
			from excludes in (
				from delimeter in Parse.Char(';')
				from notToken in Parse.String("not").Token()
				from persons in IdentifierListParse()
				select persons).Optional()
			select new RoleExpr
			{
				RoleName = roleTitle.Trim(),
				ExcludedPersons = (excludes.GetOrDefault() ?? new string[0]).ToArray()
			};

		private static Parser<SubjectGroupExpr> SubjectGroupExprParse() =>
			from token in Parse.String("group").Token()
			from groupTitle in Parse.CharExcept(new[]
			{
				';',
				'\n'
			}).AtLeastOnce().Text().Token()
			from excludes in (
				from delimeter in Parse.Char(';')
				from notToken in Parse.String("not").Token()
				from persons in IdentifierListParse()
				select persons).Optional()
			select new SubjectGroupExpr
			{
				GroupName = groupTitle.Trim(),
				ExcludedPersons = (excludes.GetOrDefault() ?? new string[0]).ToArray()
			};

		private static Parser<IDslQueryBlock> GroupDataExprParse() =>
			from orderTitle in Parse.String(DslKeywords.Group).Token()
			from identifiers in QuotedIdentifierListParse()
			select new DslGroupBlock
			{
				Items = identifiers.Select(_ =>
					new DslGroupItem
					{
						VariableName = _
					}).ToList()
			};

		private static Parser<ParameterExpr> ParameterExprParse(string mainTag) =>
			from parameter in Parse.String(mainTag).Token().Text()
			from value in Parse.LetterOrDigit.Many().Token().Text()
			select new ParameterExpr(parameter, value);

		public static Parser<ParameterExpr> DeliveryProtocolExprParse() => ParameterExprParse(DslKeywords.Protocol);

		public static Parser<ParameterExpr> NotificationReportIdParse()
			=> ParameterExprParse(DslKeywords.Report);

		public NotificationRuleExpr NotificationRuleExprParse(string query)
		{
			// TODO Event and Trigger are alternatives
			var parcer =
				from commentTrigger in CommentExprParse().Many().Token().Optional()
				from eventExpr in EventExprParse().Token().Optional()
				from triggerExpr in TimeTriggerExprParse().Token().Optional()

				from commentSubject in CommentExprParse().Many().Token().Optional()
				from subjectExpr in SubjectsExprParse().Token()

				from commentProtocol in CommentExprParse().Many().Token().Optional()
				from protocolExpr in DeliveryProtocolExprParse().Token()

				from commentTemplate in CommentExprParse().Many().Token().Optional()
				from reportExpr in NotificationReportIdParse().Token()

				from attachProtocol in CommentExprParse().Many().Token().Optional()
				from attachExpr in ParseNotificationAttachmentExpr().Token().Optional()

				select new NotificationRuleExpr
				{
					Event = eventExpr.GetOrDefault(),
					Trigger = triggerExpr.GetOrDefault(),
					Subjects = subjectExpr,
					Protocol = protocolExpr.Value,
					ReportId = long.Parse(reportExpr.Value),
					Attachments = attachExpr.GetOrDefault()
				};

			return parcer.Parse(query);
		}

		public DslDataQuery DataQueryParse(string query)
		{
			var parser =
				from entityExpr in Parse.LetterOrDigit.Many().Text().Token()
				from blocks in ParseDslBlocks.Many()
				from firstExpr in FirstQueryModeParse().Optional().Token()
				from tableExpr in ParameterExprParse(DslKeywords.Table).Optional().Token()
				select new DslDataQuery
				{
					QueryEntityName = entityExpr,
					TableKey = tableExpr.IsDefined ? tableExpr.Get().Value : null,
					Blocks = blocks.Where(_ => !(_ is CommentExprBlock)).ToArray(),
					TakeFirst = firstExpr.IsDefined && (firstExpr.Get() == FirstQueryMode.First),
					TakeFirstOrDefault = firstExpr.IsDefined && (firstExpr.Get() == FirstQueryMode.FirstOrDefault)
				};

			var dslQuery = parser.Parse(query);

			dslQuery.Parameters = dslQuery.Blocks
				.Where(_ => _ is DslFilterBlock)
				.Cast<DslFilterBlock>()
				.SelectMany(_ => _.Specification.GetParameters())
				.GroupBy(_ => _.Key)
				.Select(_ => _.First())
				.ToArray();

			return dslQuery;
		}

		private static Parser<KeyValuePairExpr> ParseKeyValuePairExpr() =>
			from key in Parse.CharExcept(':').Many().Token().Text()
			from delimiter in Parse.Char(':')
			from value in Parse.CharExcept(';').Many().Token().Text()
			from endDelimiter in Parse.Char(';')
			select new KeyValuePairExpr(key, value);

		internal static Parser<INotificationAttachBlock> ParseReportAttachmentBlockExpr() =>
			from title in Parse.String(DslKeywords.Report).Token()
			from reportId in Parse.Digit.Many().Token().Text()
			from formatTitle in Parse.String(DslKeywords.Format).Token()
			from format in Parse.Letter.Many().Token().Text()
			from parametersTitle in Parse.String(DslKeywords.Parameters).Token().Optional()
			from parameters in ParseKeyValuePairExpr().Many()
			select new ReportAttachmentBlockExpr(long.Parse(reportId), format, parameters);

		private static Parser<IEnumerable<INotificationAttachBlock>> ParseNotificationAttachmentExpr() =>
			from title in Parse.String(DslKeywords.Attach).Token()
			from blocks in ParseReportAttachmentBlockExpr().Many()
			select blocks;

		private static Parser<IDslQueryBlock> ParseDslBlocks => SelectExprParse()
			.Or(WhereExprParse())
			.Or(GroupDataExprParse())
			.Or(OrderExprParse())
			.Or(SkipExprParse())
			.Or(TakeExprParse())
			.Or(CommentExprParse());

		private static Parser<IDslQueryBlock> WhereExprParse() =>
			from whereTitle in Parse.String(DslKeywords.Where).Token()
			from condition in Parse.CharExcept('\n').Many().Text()
			select DslFilterBlockParser.ParseQuery(condition);

		private static Parser<CommentExprBlock> CommentExprParse() =>
			from start in Parse.String(DslKeywords.CommentLine)
			from message in Parse.CharExcept('\n').Many().Token().Text()
			select new CommentExprBlock(message);

		private static Parser<IDslQueryBlock> SkipExprParse() =>
			from parameter in Parse.String(DslKeywords.Skip).Token().Text()
			from value in Parse.LetterOrDigit.Many().Token().Text()
			select new DslLimitBlock
			{
				Skip = !string.IsNullOrEmpty(value) ? int.Parse(value) : (int?) null
			};

		private static Parser<IDslQueryBlock> TakeExprParse() =>
			from parameter in Parse.String(DslKeywords.Take).Token().Text()
			from value in Parse.LetterOrDigit.Many().Token().Text()
			select new DslLimitBlock
			{
				Take = !string.IsNullOrEmpty(value) ? int.Parse(value) : (int?) null
			};

		internal static Parser<DslOrderBlock> OrderExprParse() =>
			from orderTitle in Parse.String(DslKeywords.Order).Token()
			from identifiers in
				(from item in OrderExprItemParse().Token()
					from delimiter in Parse.Chars(',').Optional()
					select item).Many()
			select new DslOrderBlock
			{
				Items = identifiers.ToArray()
			};

		internal static Parser<OrderBlockItem> OrderExprItemParse() =>
			from identifier in QuotedIdentifierParse()
			from order in Parse.String(DslKeywords.Asc).Or(Parse.String(DslKeywords.Desc)).Text().Token()
			select new OrderBlockItem
			{
				OrderFieldName = identifier,
				SortOrder = order == DslKeywords.Desc ? SortOrder.Descending : SortOrder.Ascending
			};

		public static Parser<FirstQueryMode> FirstQueryModeParse() =>
			(from first in Parse.String(DslKeywords.FirstOrDefault) select FirstQueryMode.FirstOrDefault)
				.Or(from firstOrDefault in Parse.String(DslKeywords.First) select FirstQueryMode.First);

		internal static Parser<IDslQueryBlock> SelectExprParse() =>
			from selectTitle in Parse.String(DslKeywords.Select).Token()
			from items in SelectItemExprParse().Many()
			from endTitle in Parse.String(DslKeywords.SelectEnd)
			select new DslFormatBlock
			{
				Selects = items
			};

		internal static Parser<DslFormatItem> SelectItemExprParse() =>
			from endTitle in Parse.String(DslKeywords.SelectEnd).Not()
			from name in (from name in Parse.LetterOrDigit.Many().Text().Token()
				from equation in Parse.Char('=')
				select name).Optional()
			from value in Parse.CharExcept(new[] {DslKeywords.DisplayNameDelimiter, '\n'}).Many().Text().Token()
			from displayName in (
				from delimeter in Parse.Char(DslKeywords.DisplayNameDelimiter)
				from displayName in Parse.CharExcept('\n').Many().Text().Token()
				select displayName.Trim()).Optional()
			select new DslFormatItem
			{
				Name = name.GetOrDefault(),
				Value = value.Trim(),
				DisplayName = displayName.GetOrDefault()
			};

		private static Parser<DataParameterExpr> DataParameterExprParse() =>
			from next in Parse.String("conditions").Not()

			// stops parsing conditions
			from name in Parse.LetterOrDigit.Many().Text().Token()
			from equation in Parse.Char('=')
			from queryTitle in Parse.String("query").Token()
			from query in Parse.CharExcept(';').Many().Text()
			from finish in Parse.Char(';')
			select new DataParameterExpr
			{
				Name = name,
				Value = query
			};

		private static Parser<string> QuotedIdentifierParse() =>
			from quote in Parse.Char('#')
			from identifier in Parse.Letter.Or(Parse.Chars('_')).AtLeastOnce().Text()
			from quoteEnd in Parse.Char('#')
			select identifier;

		public WorkflowRuleExpr WorkflowRuleExprParse(string query)
		{
			var parcer = from eventExpr in EventExprParse().Token().Optional()
				from triggerExpr in TimeTriggerExprParse().Token().Optional()
				from actionExpr in ActionExprParse().Token()
				select new WorkflowRuleExpr
				{
					Event = eventExpr.GetOrDefault(),
					Trigger = triggerExpr.GetOrDefault(),
					Action = actionExpr
				};

			return parcer.Parse(query);
		}

		private static Parser<ParameterExpr> ActionExprParse() =>
			ParameterExprParse("action");

		public PolicyRuleExpr PolicyRuleExprParse(string query)
		{
			var parcer = from parameters in DataParameterExprParse().Many().Token().End()
				select new PolicyRuleExpr
				{
					Data = parameters.ToArray()
				};

			return parcer.Parse(query);
		}

		private static Parser<int> TimeShiftBlockParse(string delimeter) =>
			from value in Parse.Number
			from delim in Parse.String(delimeter)
			select int.Parse(value);

		private static Parser<IEnumerable<string>> QuotedIdentifierListParse() =>
			(from identifier in QuotedIdentifierParse()
				from separator in Parse.Chars(',', ' ', '\t').Optional()
				select identifier).Many();

		private static Parser<IEnumerable<string>> IdentifierListParse() =>
			(from identifier in Parse.CharExcept(new[]
			{
				',',
				'\n'
			}).AtLeastOnce().Text()
				from separator in Parse.Chars(',').Optional()
				select identifier.Trim()).Many();
	}
}