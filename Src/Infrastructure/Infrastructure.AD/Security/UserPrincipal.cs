namespace Infrastructure.AD.Security
{
	using System;

	using JetBrains.Annotations;

	using Common.Security;

	internal sealed class UserPrincipal : IUserPrincipal
	{
		private readonly ICurrentUserDataProvider _currentUserDataProvider;

		private UserInfo _info;

		public UserPrincipal([NotNull] ICurrentUserDataProvider currentUserDataProvider)
		{
			if (currentUserDataProvider == null) throw new ArgumentNullException(nameof(currentUserDataProvider));

			_currentUserDataProvider = currentUserDataProvider;
		}

		public UserInfo Info => _info ?? (_info = _currentUserDataProvider.GetOrCreate());
	}
}