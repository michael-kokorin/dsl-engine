namespace Modules.Core.Services.UI.QueryHandlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Query;
	using Common.Security;
	using Modules.Core.Contracts.UI.Dto;
	using Modules.Core.Services.UI.Queries;
	using Modules.Core.Services.UI.Renderers;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class GetNotificationQueryHandler : IDataQueryHandler<GetNotificationQuery, NotificationRuleDto>
	{
		private readonly INotificationRuleRepository _notificationRuleRepository;

		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUserPrincipal _userPrincipal;

		public GetNotificationQueryHandler(
			INotificationRuleRepository notificationRuleRepository,
			IUserAuthorityValidator userAuthorityValidator,
			IUserPrincipal userPrincipal)
		{
			_notificationRuleRepository = notificationRuleRepository;
			_userAuthorityValidator = userAuthorityValidator;
			_userPrincipal = userPrincipal;
		}

		/// <summary>
		///   Executes the specified data query.
		/// </summary>
		/// <param name="dataQuery">The data query.</param>
		/// <returns>The result of execution.</returns>
		public NotificationRuleDto Execute([NotNull] GetNotificationQuery dataQuery)
		{
			if (dataQuery == null) throw new ArgumentNullException(nameof(dataQuery));

			var notification = _notificationRuleRepository.GetById(dataQuery.Id);

			if (notification == null)
				return null;

			if (!_userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[]
				{
					Authorities.UI.Project.Settings.ViewNotifications
				},
				notification.ProjectId))
				throw new UnauthorizedAccessException();

			var notificationDto = new NotificationRuleRenderer().GetSpec().Invoke(notification);

			return notificationDto;
		}
	}
}