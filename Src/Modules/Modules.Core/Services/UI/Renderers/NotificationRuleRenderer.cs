namespace Modules.Core.Services.UI.Renderers
{
	using System;

	using Modules.Core.Contracts.UI.Dto;
	using Repository.Context;

	internal sealed class NotificationRuleRenderer : IDataRenderer<NotificationRules, NotificationRuleDto>
	{
		public Func<NotificationRules, NotificationRuleDto> GetSpec() => _ =>
			new NotificationRuleDto
			{
				DisplayName = _.DisplayName,
				Id = _.Id,
				Query = _.Query
			};
	}
}