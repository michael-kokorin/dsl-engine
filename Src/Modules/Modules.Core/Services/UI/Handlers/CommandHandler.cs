namespace Modules.Core.Services.UI.Handlers
{
	using System;

	using JetBrains.Annotations;

	using Common.Command;
	using Common.Security;
	using Common.Transaction;

	internal abstract class CommandHandler<T> : ICommandHandler<T> where T : class, ICommand
	{
		private readonly IUserAuthorityValidator _userAuthorityValidator;

		private readonly IUnitOfWork _unitOfWork;

		private readonly IUserPrincipal _userPrincipal;

		protected CommandHandler(
			[NotNull] IUserAuthorityValidator userAuthorityValidator,
			[NotNull] IUnitOfWork unitOfWork,
			[NotNull] IUserPrincipal userPrincipal)
		{
			if (userAuthorityValidator == null) throw new ArgumentNullException(nameof(userAuthorityValidator));
			if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));
			if (userPrincipal == null) throw new ArgumentNullException(nameof(userPrincipal));

			_userAuthorityValidator = userAuthorityValidator;
			_unitOfWork = unitOfWork;
			_userPrincipal = userPrincipal;
		}

		protected abstract string RequestedAuthorityName { get; }

		public void Process(T command)
		{
			if (command == null) throw new ArgumentNullException(nameof(command));

			var isAutorityValid = _userAuthorityValidator.HasUserAuthorities(
				_userPrincipal.Info.Id,
				new[] {RequestedAuthorityName},
				GetProjectIdForCommand(command));

			if (!isAutorityValid)
			{
				throw new UnauthorizedAccessException();
			}

			try
			{
				using (var transaction = _unitOfWork.BeginTransaction())
				{
					ProcessAuthorized(command);

					transaction.Commit();
				}
			}
			catch
			{
				_unitOfWork.Reset();

				throw;
			}
		}

		protected abstract long? GetProjectIdForCommand(T command);

		protected abstract void ProcessAuthorized(T command);
	}
}