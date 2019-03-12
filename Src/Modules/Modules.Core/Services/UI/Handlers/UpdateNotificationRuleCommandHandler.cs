using Modules.Core.Services.UI.Commands;

namespace Modules.Core.Services.UI.Handlers
{
	using JetBrains.Annotations;

	using Common.Security;
	using Common.Transaction;
	using Repository.Repositories;

	[UsedImplicitly]
	internal sealed class UpdateNotificationRuleCommandHandler : CommandHandler<UpdateNotificationRuleCommand>
	{
		private readonly INotificationRuleRepository _notificationRuleRepository;

		public UpdateNotificationRuleCommandHandler(
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUserPrincipal userPrincipal,
			[NotNull] INotificationRuleRepository notificationRuleRepository,
			[NotNull] IUnitOfWork unitOfWork)
			: base(userAuthorityValidator, unitOfWork, userPrincipal)
		{
			_notificationRuleRepository = notificationRuleRepository;
		}

		protected override string RequestedAuthorityName => Authorities.UI.Project.Settings.EditNotifications;

		protected override long? GetProjectIdForCommand(UpdateNotificationRuleCommand command) =>
			_notificationRuleRepository.GetById(command.RuleId)?.ProjectId;

		protected override void ProcessAuthorized(UpdateNotificationRuleCommand command)
		{
			var rule = _notificationRuleRepository.GetById(command.RuleId);

			rule.Query = command.Query;

			_notificationRuleRepository.Save();
		}
	}
}