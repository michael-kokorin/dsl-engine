namespace Infrastructure.AD.Security
{
	using System;

	using JetBrains.Annotations;

	using Common.Security;

	[UsedImplicitly]
	internal sealed class CurrentUserDataProvider : ICurrentUserDataProvider
	{
		private readonly IAdUserInfoProvider _adUserInfoProvider;

		private readonly IUserInfoProvider _userInfoProvider;

		private readonly IUserPrincipalProvider _userPrincipalProvider;

		public CurrentUserDataProvider([NotNull] IAdUserInfoProvider adUserInfoProvider,
			[NotNull] IUserInfoProvider userInfoProvider,
			[NotNull] IUserPrincipalProvider userPrincipalProvider)
		{
			if (adUserInfoProvider == null) throw new ArgumentNullException(nameof(adUserInfoProvider));
			if (userInfoProvider == null) throw new ArgumentNullException(nameof(userInfoProvider));
			if (userPrincipalProvider == null) throw new ArgumentNullException(nameof(userPrincipalProvider));

			_adUserInfoProvider = adUserInfoProvider;
			_userInfoProvider = userInfoProvider;
			_userPrincipalProvider = userPrincipalProvider;
		}

		public UserInfo GetOrCreate()
		{
			var currentPrincipal = _userPrincipalProvider.Get();

			var userInfo = _userInfoProvider.Get(currentPrincipal.Sid);

			if (userInfo != null)
			{
				if (userInfo.Login.Equals(currentPrincipal.Name, StringComparison.InvariantCultureIgnoreCase))
					return userInfo;

				var existsUserAdInfo = _adUserInfoProvider.Get(currentPrincipal.Sid);

				_userInfoProvider.Update(userInfo.Id,
					existsUserAdInfo.Login,
					existsUserAdInfo.DisplayName,
					existsUserAdInfo.Email);

				return userInfo;
			}

			var newUserAdInfo = _adUserInfoProvider.Get(currentPrincipal.Sid);

			return _userInfoProvider.Create(
				new UserInfo
				{
					DisplayName = newUserAdInfo.DisplayName,
					Email = newUserAdInfo.Email,
					Login = newUserAdInfo.Login,
					Sid = currentPrincipal.Sid
				});
		}
	}
}